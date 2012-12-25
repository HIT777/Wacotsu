using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Timers;
using System.Xml.Linq;

namespace Wacotsu
{
	public class SeatFetcher
	{
		private WebClientWithCookie client;

		public SeatFetcher(string userSession)
		{
			this.client = new WebClientWithCookie();
			this.client.Encoding = UTF8Encoding.UTF8;
			var cookie = new Cookie("user_session", userSession, "/",  "nicovideo.jp");
			this.client.CookieContainer.Add(new Uri("http://nicovideo.jp"), cookie);
		}

		public FetchResult Fetch(Live live)
		{
			var accessUrl = string.Format("http://watch.live.nicovideo.jp/api/getplayerstatus?v={0}", live.Id);
			var responseString = client.DownloadString(accessUrl);
			var xml = XElement.Parse(responseString);
			var status = xml.Attribute("status").Value;
			if (status != "ok")
			{
				var errorCode = xml.Descendants("code").First().Value;
				status = errorCode;
			}
			var nowTime = TimeUtil.UnixTimeToDateTime(xml.Attribute("time").Value).ToLocalTime();
			var result = new FetchResult();
			result.Time = nowTime;
			switch (status)
			{
			case "ok":
				result.Status = Status.Ok;
				var seatLabel = xml.Descendants("room_label").First().Value;
				var seatNumber = int.Parse(xml.Descendants("room_seetno").First().Value);
				var seat = new Seat { Label = seatLabel, Number = seatNumber };
				result.Seat = seat;
				break;

			case "comingsoon":
				result.Status = Status.ComingSoon;
				break;

			case "notlogin":
				result.Status = Status.NotLogin;
				break;

			case "noauth":
				result.Status = Status.NoAuth;
				break;

			case "closed":
				result.Status = Status.Closed;
				break;

			case "require_community_member":
				result.Status = Status.RequireCommunityMember;
				break;

			case "notfound":
				result.Status = Status.NotFound;
				break;

			default:
				throw new Exception("unknown fetch result status: " + status);
			}
			return result;
		}
	}
}
