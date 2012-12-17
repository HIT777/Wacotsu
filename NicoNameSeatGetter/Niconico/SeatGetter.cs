using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Wacotsu.Niconico
{
	public class SeatGetter : IDisposable
	{
		/// <summary>
		/// 
		/// </summary>
		public Seat Seat { get; protected set; }

		/// <summary>
		/// 
		/// </summary>
		private Net.WebClientWithCookie client;

		/// <summary>
		/// 
		/// </summary>
		public SeatGetter(User user)
		{
			this.client = new Net.WebClientWithCookie();
			this.client.Encoding = UTF8Encoding.UTF8;
			var cookie = new System.Net.Cookie("user_session", user.SessionId, "/", ".nicovideo.jp");
			this.client.CookieContainer.Add(new Uri("http://nicovideo.jp"), cookie);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="live"></param>
		/// <returns>座席が確保できた場合は確保した座席の情報　まだ開場していなかった場合はnull</returns>
		public Seat GetSeat(Live live)
		{
			var accessUrl = string.Format("http://watch.live.nicovideo.jp/api/getplayerstatus?v={0}", live.Id);
			var xmlString = this.client.DownloadString(accessUrl);
			var xml = XElement.Parse(xmlString);
			var status = xml.Attribute("status").Value;

			if (status != "ok")
			{
				var code = xml.Descendants("code").First().Value;
				if (code == "comingsoon")
				{
					return null;
				}
				else
				{
					throw new SeatFailedException { ErrorCode = code };
				}
			}

			var seatLabel = xml.Descendants("room_label").First().Value;
			var seatNumber = int.Parse(xml.Descendants("room_seetno").First().Value);
			return new Seat { Label = seatLabel, Number = seatNumber };
		}

		/// <summary>
		/// 
		/// </summary>
		public void Dispose()
		{
			if (this.client != null)
			{
				this.client.Dispose();
			}
		}
	}
}
