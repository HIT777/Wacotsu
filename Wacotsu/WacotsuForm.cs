using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Wacotsu
{
	public partial class WacotsuForm : Form
	{
		/// <summary>
		/// 本体
		/// </summary>
		private Wacotsu wacotsu;

		/// <summary>
		/// 
		/// </summary>
		private VendorBrowser browser;

		public WacotsuForm()
		{
			InitializeComponent();
			if (Properties.Settings.Default.StartPosition.IsEmpty)
			{
				this.StartPosition = FormStartPosition.CenterScreen;
			}
			else
			{
				this.DesktopLocation = Properties.Settings.Default.StartPosition;
			}
		}

		private void WacotsuForm_Load(object sender, EventArgs e)
		{
			try
			{
				this.init();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
				Application.Exit();
			}
		}

		/// <summary>
		/// 初期化
		/// </summary>
		private async void init()
		{
			this.clockTimer_Tick(this.clockTimer, new EventArgs());
			this.browser = new VendorBrowsers.Chrome();
			var sessionCookie = browser.GetCookie(new Uri("http://nicovideo.jp/"), "user_session");
			if (sessionCookie == null)
			{
				throw new Exception("標準のブラウザでニコニコ動画にログインしてください");
			}
			this.wacotsu = new Wacotsu(sessionCookie.Value);
			this.wacotsu.Success += this.wacotsu_Success;
			this.wacotsu.Failed += this.wacotsu_Failed;

			this.statusBarLabel.Text = "予約可能な放送一覧を取得中...";
			this.availableLiveListView.BeginUpdate();
			await Task.Run(() =>
			{
				try
				{
					foreach (var live in LiveFetcher.Fetch())
					{
						var item = this.createLiveListViewItem(live);
						this.Invoke((MethodInvoker)delegate()
						{
							this.thumbnailImageList.Images.Add(live.Id, live.Thumbnail);
							this.availableLiveListView.Items.Add(item);
						});
					}
				}
				catch (ObjectDisposedException)
				{
					Application.Exit();
				}
				catch (Exception ex)
				{
					MessageBox.Show(ex.Message);
					Application.Exit();
				}
			});
			this.availableLiveListView.EndUpdate();
			this.statusBarLabel.Text = string.Empty;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void wacotsu_Success(object sender, SuccessEventArgs e)
		{
			var item = this.createLiveListViewItem(e.Live, e.Seat);
			this.Invoke((MethodInvoker)delegate()
			{
				// バルーンチップ&音声通知
				if (this.ballonTipEnabledCheckBox.Checked)
				{
					System.Media.SystemSounds.Beep.Play();
					this.notifyIcon.ShowBalloonTip(3000, "<<座席確保>>" + e.Live.Title, e.Seat.ToString(), ToolTipIcon.Info);
				}
				// フォームを点滅させて通知する
				this.Activate();
				this.gotLiveListView.Items.Add(item);
			});
			this.removeItemFromReservedListView(e.Live);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void wacotsu_Failed(object sender, FailedEventArgs e)
		{
			string message = null;
			switch (e.Status)
			{
			case Status.Closed:
				message = "すでに終了した放送です";
				break;

			case Status.NoAuth:
				message = "この放送を視聴するには特別な認証が必要です";
				break;

			case Status.NotFound:
				message = "放送が見つかりませんでした";
				break;

			case Status.NotLogin:
				message = "ログイン状態が解除されていて放送の情報を取得できません。\r\n標準のブラウザでニコニコ動画にログインしてからもう一度このアプリを再起動してください";
				break;

			case Status.RequireCommunityMember:
				message = "この放送を視聴するにはコミュニティに入会する必要があります";
				break;
			}
			MessageBox.Show(message);
			this.removeItemFromReservedListView(e.Live);
		}

		/// <summary>
		/// 予約リストから指定した放送のアイテムを削除する
		/// </summary>
		/// <param name="live"></param>
		private void removeItemFromReservedListView(Live live)
		{
			this.Invoke((MethodInvoker)delegate()
			{
				var removedItemIndex = -1;
				foreach (var item in this.reservedLiveListView.Items.Cast<ListViewItem>())
				{
					var currentLive = item.Tag as Live;
					if (live.Id == currentLive.Id)
					{
						removedItemIndex = item.Index;
						break;
					}
				}
				if (removedItemIndex >= 0)
				{
					this.reservedLiveListView.Items.RemoveAt(removedItemIndex);
				}
			});
		}

		/// <summary>
		/// リストビューに表示するための放送情報を作成する
		/// </summary>
		/// <param name="live"></param>
		/// <param name="seat"></param>
		/// <returns></returns>
		private ListViewItem createLiveListViewItem(Live live, Seat seat = null)
		{
			var item = new ListViewItem();
			item.Tag = live;

			string label = null;
			if (seat == null)
			{
				var leftTime = live.OpenTime - DateTime.Now;
				if (leftTime.TotalHours < 3)
				{
					label = live.GetLeftTimeString();
				}
				else
				{
					label = live.OpenTime.ToString("MM/dd(ddd) HH:mm開場");
				}
			}
			else
			{
				label = seat.ToString();
			}
			item.Text = string.Format("<<{0}>>\r\n{1}", label, live.Title);
			item.ToolTipText = string.Format("開場: {0}", live.OpenTime.ToString("MM/dd HH:mm"));
			item.ImageKey = live.Id;
			return item;
		}

		/// <summary>
		/// リストビューをダブルクリックしたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void listView_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			try
			{
				var listView = sender as ListView;
				foreach (var item in listView.SelectedItems.Cast<ListViewItem>())
				{
					var live = item.Tag as Live;

					// 予約可能な放送一覧の放送を予約
					if (listView == this.availableLiveListView)
					{
						var addItem = this.createLiveListViewItem(live);
						if (this.wacotsu.Reserve(live))
						{
							this.reservedLiveListView.Items.Add(addItem);
						}
					}

					// 予約した放送一覧の放送を取消
					if (listView == this.reservedLiveListView)
					{
						this.reservedLiveListView.Items.Remove(item);
						this.wacotsu.Cancel(live);
					}

					// 確保済の放送をブラウザで開く
					if (listView == this.gotLiveListView)
					{
						this.browser.Open(live.WatchUri);
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void listView_MouseClick(object sender, MouseEventArgs e)
		{
			if (e.Button != MouseButtons.Right)
			{
				return;
			}
			var listView = sender as ListView;
			foreach (var item in listView.SelectedItems.Cast<ListViewItem>())
			{
				var live = item.Tag as Live;
				var gateUri = new Uri(string.Format("http://live.nicovideo.jp/gate/{0}", live.Id));
				this.browser.Open(gateUri);
			}
		}

		private void WacotsuForm_ClientSizeChanged(object sender, EventArgs e)
		{
			if (this.taskTrayEnabledCheckBox.Checked && this.WindowState == FormWindowState.Minimized)
			{
				this.ShowInTaskbar = false;
				this.notifyIcon.ShowBalloonTip(1000, string.Empty, "タスクトレイに格納されます", ToolTipIcon.None);
			}
		}

		private void notifyIcon_MouseClick(object sender, MouseEventArgs e)
		{
			if (this.WindowState == FormWindowState.Minimized)
			{
				this.WindowState = FormWindowState.Normal;
				this.ShowInTaskbar = true;
			}
		}

		private void updateLiveListViewTime(ListView listView)
		{
			foreach (var item in listView.Items.Cast<ListViewItem>())
			{
				var live = item.Tag as Live;
				string label = null;
				var leftTime = live.OpenTime - DateTime.Now;
				if (leftTime.TotalHours < 3)
				{
					label = live.GetLeftTimeString();
				}
				else
				{
					label = live.OpenTime.ToString("MM/dd(ddd) HH:mm開場");
				}
				item.Text = string.Format("<<{0}>>\r\n{1}", label, live.Title);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void clockTimer_Tick(object sender, EventArgs e)
		{
			this.updateLiveListViewTime(this.availableLiveListView);
			this.updateLiveListViewTime(this.reservedLiveListView);
			this.clockLabel.Text = DateTime.Now.ToString("yyyy/MM/dd（ddd）HH:mm");
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void WacotsuForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			this.notifyIcon.Visible = false;
			Properties.Settings.Default.StartPosition = this.DesktopLocation;
			Properties.Settings.Default.Save();
		}
	}
}
