using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace Wacotsu
{
	/// <summary>
	/// クッキーを保持することができるWebClient
	/// </summary>
	public class WebClientWithCookie : WebClient
	{
		/// <summary>
		/// クッキーを保持するコンテナ
		/// </summary>
		public CookieContainer CookieContainer;

		/// <summary>
		/// 
		/// </summary>
		public WebClientWithCookie() : base()
		{
			// Proxyが有効の場合初回接続時に非常に遅くなるので無効にする
			this.Proxy = null;
			this.CookieContainer = new CookieContainer();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="address"></param>
		/// <returns></returns>
		protected override WebRequest GetWebRequest(Uri address)
		{
			var request = base.GetWebRequest(address);
			if (request is HttpWebRequest)
			{
				((HttpWebRequest)request).CookieContainer = this.CookieContainer;
			}
			return request;
		}
	}
}
