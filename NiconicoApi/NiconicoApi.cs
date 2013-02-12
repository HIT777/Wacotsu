using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Xml.Linq;
using NiconicoApi.Extensions;
using ScrapySharp.Extensions;

namespace NiconicoApi 
{
	public class NiconicoApi
	{
		/// <summary>
		/// niconicoAPIにアクセスするHTTPクライアント 
		/// </summary>
		private HttpClient httpClient;

		public NiconicoApi(string userSessionId)
		{
			// 認証情報のクッキーを作る
			// 第3引数と第4引数のpath, domainは必ず指定すること
			var userSessionCookie = new Cookie("user_session", userSessionId, "/", "nicovideo.jp");
			var cookieContainer = new CookieContainer();
			cookieContainer.Add(userSessionCookie);

			// HTTPハンドラーにクッキーを登録する
			var httpHandler = new HttpClientHandler();
			httpHandler.CookieContainer = cookieContainer;

			this.httpClient = new HttpClient(httpHandler);
		}

		/// <summary>
		/// niconicoのサーバー時間を取得する
		/// </summary>
		/// <returns></returns>
		public async Task<DateTime> GetServerTimeAsync()
		{
			// ニコ生アラートAPIにアクセスしたら結果に関わらず必ずサーバー時間を返してくれるのでそれを利用する
			var requestUri = new Uri("http://live.nicovideo.jp/api/getalertstatus");
			var xml = await httpClient.GetXmlAsync(requestUri);
			var unixTimeString = xml.Root.Attribute("time").Value;
			return TimeUtil.UnixTimeToDateTime(unixTimeString).ToLocalTime();
		}

		/// <summary>
		/// 指定した生放送の状態を取得する
		/// </summary>
		/// <param name="liveId">取得したい生放送のID　（例）lv11202010</param>
		/// <returns>正常に取得できたらLiveStatusを返す
		/// まだ放送が開場していないならnullを返す
		/// エラーが起こったら例外を投げる</returns>
		public async Task<Live.Status> GetLiveStatusAsync(string liveId)
		{
			var requestUri = new Uri(string.Format("http://watch.live.nicovideo.jp/api/getplayerstatus?v={0}", liveId));
			var xml = await httpClient.GetXmlAsync(requestUri);
			var status = xml.Root.Attribute("status").Value;
			
			if (status != "ok") {
				var errorCodeString = xml.Descendants("error").Descendants("code").First().Value;
				if (errorCodeString == "comingsoon") {
					return null;
				}
				throw new Live.StatusException(errorCodeString);
			}

			var roomName = xml.Descendants("room_label").First().Value;
			var seatNumber = int.Parse(xml.Descendants("room_seetno").First().Value);

			return new Live.Status { RoomName = roomName, SeatNumber = seatNumber };
		}

		/// <summary>
		/// 最近の生放送を取得する
		/// </summary>
		/// <returns></returns>
		public async Task<IReadOnlyCollection<Live.Info>> GetRecentLiveInfos()
		{
			var requestUri = new Uri("http://ch.nicovideo.jp/menu/anime/");
			var html = await httpClient.GetHtmlAsync(requestUri);

			var liveInfos = new List<Live.Info>();
			foreach (var node in html.DocumentNode.CssSelect("#sec_live li")) {
				var liveId = System.Text.RegularExpressions.Regex.Match(node.CssSelect(".symbol a").First().Attributes["href"].Value, @"(lv[0-9]+)").Groups[1].Value;
				var liveTitle = node.CssSelect(".tit a").First().InnerText;
				var imageUri = new Uri(node.CssSelect(".symbol img").First().Attributes["src"].Value);
				var openTime = (node.CssSelect(".detail .date strong").Count() > 0) ?
					parseAnimeString(node.CssSelect(".detail .date strong").First().InnerText) :
					DateTime.MinValue;	

				var liveInfo = new Live.Info { Id = liveId, Title = liveTitle, ImageUri = imageUri, OpenTime = openTime };
				liveInfos.Add(liveInfo);
			}
			return liveInfos;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="source"></param>
		/// <returns></returns>
		private DateTime parseAnimeString(string source)
		{
			var sources = source.Split(' ');
			var baseDateTime = DateTime.Parse(sources[0], System.Globalization.CultureInfo.InvariantCulture);
			var hour = int.Parse(sources[1].Split(':')[0]);
			var minute = int.Parse(sources[1].Split(':')[1]);
			var addedSpan = new TimeSpan(hour, minute, 0);
			return baseDateTime.Add(addedSpan);
		}
	}
}
