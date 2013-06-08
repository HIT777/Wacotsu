using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Xml.Linq;
using HtmlAgilityPack;

namespace NiconicoApi.Extensions
{
	public static class HttpClientExtension
	{
		/// <summary>
		/// HTTPを使ってHtmlデータを取得する
		/// </summary>
		/// <param name="client"></param>
		/// <param name="requestUri"></param>
		/// <returns></returns>
		public static Task<HtmlDocument> GetHtmlAsync(this HttpClient client, Uri requestUri)
		{
			return Task<HtmlDocument>.Run(() => {
				var responseString = client.GetStringAsync(requestUri).Result;
				var htmlDocument = new HtmlDocument();
				htmlDocument.LoadHtml(responseString);
				return htmlDocument;
			});
		}
		
		/// <summary>
		/// HTTPを使ってXMLデータを取得する
		/// </summary>
		/// <param name="client"></param>
		/// <param name="requestUri"></param>
		/// <returns></returns>
		public static Task<XDocument> GetXmlAsync(this HttpClient client, Uri requestUri)
		{
			return Task<XDocument>.Run(() => {
				var responseString = client.GetStringAsync(requestUri).Result;
				return XDocument.Parse(responseString);
			});
		}
	}
}
