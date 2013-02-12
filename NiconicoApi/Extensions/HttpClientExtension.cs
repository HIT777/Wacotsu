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
		public async static Task<HtmlDocument> GetHtmlAsync(this HttpClient client, Uri requestUri)
		{
			var responseString = await client.GetStringAsync(requestUri);
			var htmlDocument = new HtmlDocument();
			htmlDocument.LoadHtml(responseString);
			return htmlDocument;
		}
		
		/// <summary>
		/// HTTPを使ってXMLデータを取得する
		/// </summary>
		/// <param name="client"></param>
		/// <param name="requestUri"></param>
		/// <returns></returns>
		public async static Task<XDocument> GetXmlAsync(this HttpClient client, Uri requestUri)
		{
			var responseString = await client.GetStringAsync(requestUri);
			return XDocument.Parse(responseString);
		}
	}
}
