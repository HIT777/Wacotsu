using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Xml.Linq;

namespace Wacotsu.Niconico
{
	/// <summary>
	/// ニコニコ動画にログインするための機能を提供するもの
	/// </summary>
	public static class LoginUnit
	{
		/// <summary>
		/// 
		/// </summary>
		public static Uri Uri { get { return new Uri("http://nicovideo.jp"); } }

		/// <summary>
		/// ニコニコのログインアドレス
		/// </summary>
		const string LoginUrl = "https://secure.nicovideo.jp/secure/login?site=niconico";

		/// <summary>
		/// 
		/// </summary>
		/// <param name="mail"></param>
		/// <param name="password"></param>
		/// <returns>ログイン中のユーザー情報</returns>
		public static User Login(string mail, string password)
		{
			using (var client = new Net.WebClientWithCookie())
			{
				client.UploadValues(LoginUnit.LoginUrl, new System.Collections.Specialized.NameValueCollection
				{
					{"mail", mail},
					{"password", password},
				});
				var niconicoDomainUri = new Uri("http://nicovideo.jp");
				var sessionId = client.CookieContainer.GetCookies(niconicoDomainUri)["user_session"].Value;
				if (sessionId == null)
				{
					throw new AuthException("ニコニコへのログインに失敗");
				}
				return new User { SessionId = sessionId };
			}
		}
	}
}
