using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiconicoApiTest
{
	class Program
	{
		static void Main(string[] args)
		{
			Test();
		}

		static async void Test()
		{
			var chrome = new VendorBrowser.VendorBrowsers.Chrome();
			var userSessionId = chrome.GetCookie("nicovideo.jp", "user_session").Value;

			var nico = new NiconicoApi.NiconicoApi(userSessionId);

			var serverTime =  nico.GetServerTimeAsync().Result;
			Console.WriteLine("server time: {0}", serverTime);

			var liveId = "lv123792201";
			var liveStatus = nico.GetLiveStatusAsync(liveId).Result;
			Console.WriteLine("{0} - {1}", liveStatus.RoomName, liveStatus.SeatNumber);
		}
	}
}
