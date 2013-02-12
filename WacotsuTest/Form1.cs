using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WacotsuTest
{
	public partial class Form1 : Form
	{
		private Wacotsu.Wacotsu wacotsu;

		public Form1()
		{
			InitializeComponent();
		}

		private async void Form1_Load(object sender, EventArgs e)
		{
			var chrome = new VendorBrowser.VendorBrowsers.Chrome();
			var userSessionId = chrome.GetCookie("nicovideo.jp", "user_session").Value;
			var api = new NiconicoApi.NiconicoApi(userSessionId);

			wacotsu = new Wacotsu.Wacotsu(api, 3000);
			wacotsu.TimerElapsed += wacotsu_TimerElapsed;
			wacotsu.Success += wacotsu_Success;
			wacotsu.Failed += wacotsu_Failed;

			userSessionLabel.Text += userSessionId;

			liveInfoListBox.DisplayMember = "Title";
			foreach (var info in await api.GetRecentLiveInfos()) {
				liveInfoListBox.Items.Add(info);
			}
		}

		void wacotsu_Failed(object sender, Wacotsu.FailedEventArgs e)
		{
			MessageBox.Show(e.FailReason.ToString());
		}

		void wacotsu_Success(object sender, Wacotsu.SuccessEventArgs e)
		{
			this.Invoke((MethodInvoker)delegate() {
				logTextBox.AppendText(
					string.Format("【確保成功】{0}: {1}\r\n", e.LiveId, e.LiveStatus)
				);
			});
		}

		void wacotsu_TimerElapsed(object sender, Wacotsu.ElapsedEventArgs e)
		{
			this.Invoke((MethodInvoker)delegate() {
				logTextBox.AppendText(
					string.Format("{0}まであと{1}\r\n", e.LiveId, e.leftTime)
				);
			});
		}

		private void reserveButton_Click(object sender, EventArgs e)
		{
			if (string.IsNullOrWhiteSpace(liveIdTextBox.Text)) {
				return;
			}

			var selectedLiveInfo = liveInfoListBox.SelectedItem as NiconicoApi.Live.Info;
			wacotsu.Reserve(selectedLiveInfo.Id, selectedLiveInfo.OpenTime);
			logTextBox.AppendText(
				string.Format("【予約】{0} - {1}\r\n", selectedLiveInfo.Id, selectedLiveInfo.Title)
			);
		}

		private void liveInfoListBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			var selectedLiveInfo = liveInfoListBox.SelectedItem as NiconicoApi.Live.Info;
			liveIdTextBox.Text = selectedLiveInfo.Id;
		}
	}
}
