using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Http;

namespace WacotsuForm
{
	public partial class WacotsuForm : Form
	{
		/// <summary>
		/// ニコニコAPI
		/// </summary>
		private NiconicoApi.NiconicoApi api;

		/// <summary>
		/// 本体
		/// </summary>
		private Wacotsu.Wacotsu wacotsu;

		/// <summary>
		/// ログイン情報を所持しているブラウザ
		/// </summary>
		private VendorBrowser.VendorBrowser browser;

		/// <summary>
		/// 放送情報のキャッシュ
		/// </summary>
		private Dictionary<string, NiconicoApi.Live.Info> liveInfos;

		/// <summary>
		/// サーバー時間のキャッシュ
		/// </summary>
		private static DateTime serverTime;

		/// <summary>
		/// 
		/// </summary>
		public WacotsuForm()
		{
			InitializeComponent();
			liveInfos = new Dictionary<string, NiconicoApi.Live.Info>();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void WacotsuForm_Load(object sender, EventArgs e)
		{
			init();
		}

		/// <summary>
		/// 初期化
		/// </summary>
		private void init()
		{
			initWindowLocation();

			// ブラウザのニコニコ認証情報を取り出してAPIを作る
			browser = new VendorBrowser.VendorBrowsers.Chrome();
			var userSessionId = browser.GetCookie("nicovideo.jp", "user_session").Value;
			api = new NiconicoApi.NiconicoApi(userSessionId);

			// APIを作ったら、Wacotsu本体を作る
			wacotsu = new Wacotsu.Wacotsu(api);
			wacotsu.Success += wacotsu_Success;
			wacotsu.Failed += wacotsu_Failed;

			loadAvailableLiveInfoList();
			initClock();
		}

		/// <summary>
		/// ウィンドウの初期位置を設定する
		/// </summary>
		private void initWindowLocation()
		{
			if (Properties.Settings.Default.StartPosition.IsEmpty) {
				StartPosition = FormStartPosition.CenterScreen;
			}
			else {
				DesktopLocation = Properties.Settings.Default.StartPosition;
			}
		}

		/// <summary>
		/// 時計を初期化する
		/// </summary>
		private void initClock()
		{
			var clockTimer = new Timer(this.components);
			clockTimer.Enabled = true;
			clockTimer.Interval = 30000;
			clockTimer.Tick += clockTimer_Tick;
			clockTimer_Tick(clockTimer, new EventArgs());
			clockTimer.Start();
		}

		private async void clockTimer_Tick(object sender, EventArgs e)
		{
			serverTime = await api.GetServerTimeAsync();
			clockLabel.Text = serverTime.ToString("yyyy/MM/dd（ddd）HH:mm");
			updateListView();
		}

		/// <summary>
		/// 予約可能な放送一覧をWebから読み込む
		/// </summary>
		private async void loadAvailableLiveInfoList()
		{
			var httpClient = new HttpClient();

			availableLiveListView.BeginUpdate();

			foreach (var liveInfo in await api.GetRecentLiveInfos()) {
				// 画像URLから画像データを作成する
				if (thumbnailImageList.Images.ContainsKey(liveInfo.Id) == false) {
					var image = Image.FromStream(await httpClient.GetStreamAsync(liveInfo.ImageUri));
					thumbnailImageList.Images.Add(liveInfo.Id, image);
				}
				liveInfos.Add(liveInfo.Id, liveInfo);
				// 予約可能リストに追加する
				availableLiveListView.Items.Add(liveInfo.Id, liveInfo.Title, liveInfo.Id);
			}

			updateListView();

			availableLiveListView.EndUpdate();
		}

		/// <summary>
		/// Wacotsuが座席確保に成功した時
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void wacotsu_Success(object sender, Wacotsu.SuccessEventArgs e)
		{
			var liveInfo = liveInfos[e.LiveId];
			Invoke((MethodInvoker)delegate() {
				// バルーンチップ&音声通知
				if (ballonTipEnabledCheckBox.Checked) {
					System.Media.SystemSounds.Beep.Play();
					var tipTitle = string.Format("<<座席確保>>{0}", liveInfo.Title);
					var tipDescription = e.LiveStatus.ToString();
					notifyIcon.ShowBalloonTip(3000, tipTitle, tipDescription, ToolTipIcon.Info);
				}
				// ブラウザで開く
				browser.Open(liveInfo.WatchUri);

				// 確保成功リストに移動する
				var itemText = string.Format("<<{0}>>\r\n{1}", e.LiveStatus, liveInfo.Title);
				succeededLiveListView.Items.Add(e.LiveId, itemText, e.LiveId);
				reservedLiveListView.Items.RemoveByKey(e.LiveId);
			});
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void wacotsu_Failed(object sender, Wacotsu.FailedEventArgs e)
		{
			string message = null;
			switch (e.FailReason)
			{
			case NiconicoApi.Live.StatusErrorReason.Closed:
				message = "すでに終了した放送です";
				break;

			case NiconicoApi.Live.StatusErrorReason.NoAuth:
				message = "この放送を視聴するには特別な認証が必要です";
				break;

			case NiconicoApi.Live.StatusErrorReason.NotFound:
				message = "放送が見つかりませんでした";
				break;

			case NiconicoApi.Live.StatusErrorReason.NotLogin:
				message = "ログイン状態が解除されていて放送の情報を取得できません。\r\n標準のブラウザでニコニコ動画にログインしてからもう一度このアプリを再起動してください";
				break;

			case NiconicoApi.Live.StatusErrorReason.RequireCommunityMember:
				message = "この放送を視聴するにはコミュニティに入会する必要があります";
				break;

			default:
				message = "原因不明のエラーです";
				break;
			}
			MessageBox.Show(message);
			reservedLiveListView.Items.RemoveByKey(e.LiveId);
		}

		/// <summary>
		/// リストビューをダブルクリックしたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void listView_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			var listView = sender as ListView;
			foreach (var item in listView.SelectedItems.Cast<ListViewItem>()) {
				var liveInfo = liveInfos[item.Name];

				// 予約可能な放送一覧の放送を予約
				if (listView == availableLiveListView)	{
					// すでに予約済・確保済の場合は中断
					if (reservedLiveListView.Items.ContainsKey(liveInfo.Id) ||
						succeededLiveListView.Items.ContainsKey(liveInfo.Id)) {
						MessageBox.Show(liveInfo.Title + " はすでに予約または確保されている放送です");
						continue;
					}
					wacotsu.Reserve(liveInfo.Id, liveInfo.OpenTime);
					
					// 予約済リストに追加
					reservedLiveListView.Items.Add(item.Name, item.Text, item.ImageKey);
				}

				// 予約済の放送を取消
				if (listView == reservedLiveListView) {
					reservedLiveListView.Items.Remove(item);
					wacotsu.Cancel(liveInfo.Id);
				}

				// 確保済の放送をブラウザで開く
				if (listView == succeededLiveListView) {
					browser.Open(liveInfo.WatchUri);
				}
			}
		}

		/// <summary>
		/// リストビュー内の放送情報を右クリックしたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void listView_MouseClick(object sender, MouseEventArgs e)
		{
			// 右クリック以外のクリックならば中断
			if (e.Button != MouseButtons.Right) {
				return;
			}

			var listView = sender as ListView;
			foreach (var item in listView.SelectedItems.Cast<ListViewItem>()) {
				var liveInfo = liveInfos[item.Name];
				browser.Open(liveInfo.GateUri);
			}
		}

		/// <summary>
		/// メインウィンドウの状態が変更されたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void WacotsuForm_ClientSizeChanged(object sender, EventArgs e)
		{
			if (taskTrayEnabledCheckBox.Checked && WindowState == FormWindowState.Minimized) {
				ShowInTaskbar = false;
				notifyIcon.ShowBalloonTip(1000, string.Empty, "タスクトレイに格納されます", ToolTipIcon.None);
			}
		}

		/// <summary>
		/// タスクバーの通知アイコンがクリックされたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void notifyIcon_MouseClick(object sender, MouseEventArgs e)
		{
			if (WindowState == FormWindowState.Minimized) {
				WindowState = FormWindowState.Normal;
				ShowInTaskbar = true;
			}
		}

		private void updateListView()
		{
			updateLiveListViewTime(availableLiveListView);
			updateLiveListViewTime(reservedLiveListView);
		}

		private void updateLiveListViewTime(ListView listView)
		{
			foreach (var item in listView.Items.Cast<ListViewItem>())
			{
				var liveInfo = liveInfos[item.Name];
				var leftTimeSpan = liveInfo.OpenTime - serverTime;
				var itemText = liveInfo.OpenTime.ToString("MM/dd(ddd) HH:mm開場");
				if (leftTimeSpan.TotalSeconds <= 0) {
					itemText = "上映中";
				}
				else if (leftTimeSpan.TotalMinutes < 1) {
					itemText = "もうすぐ開場";
				}
				else if (leftTimeSpan.TotalHours < 3) {
					itemText = NiconicoApi.TimeUtil.GetLeftTimeSpanString(leftTimeSpan);
				}

				item.Text = string.Format("<<{0}>>\r\n{1}", itemText, liveInfo.Title);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void WacotsuForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			notifyIcon.Visible = false;
			Properties.Settings.Default.StartPosition = DesktopLocation;
			Properties.Settings.Default.Save();
		}
	}
}
