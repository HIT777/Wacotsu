using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Drawing;
using System.Net;
using HtmlAgilityPack;
using ScrapySharp.Extensions;

namespace Wacotsu
{
	public static class LiveFetcher
	{
		/// <summary>
		/// 最近の生放送を取得する
		/// </summary>
		/// <returns></returns>
		public static IEnumerable<Live> Fetch()
		{
			using (var client = new WebClientWithCookie())
			{
				client.Encoding = UTF8Encoding.UTF8;
				var requestUrl = "http://ch.nicovideo.jp/menu/anime/";
				var htmlString = client.DownloadString(requestUrl);
				var htmlDocument = new HtmlDocument();
				htmlDocument.LoadHtml(htmlString);
				var rootNode = htmlDocument.DocumentNode;
				foreach (var itemNode in rootNode.CssSelect("#sec_live li"))
				{
					var imageUri = new Uri(itemNode.CssSelect(".symbol img").First().Attributes["src"].Value);
					Image thumbnail;
					using (var stream = HttpWebRequest.Create(imageUri).GetResponse().GetResponseStream())
					{
						thumbnail = Image.FromStream(stream);
					}
					var id = Regex.Match(itemNode.CssSelect(".symbol a").First().Attributes["href"].Value, @"(lv[0-9]+)").Groups[1].Value;
					var title = itemNode.CssSelect(".tit a").First().InnerText;
					var openTime = DateTime.Now;
					if (itemNode.CssSelect(".detail .date strong").Count() > 0)
					{
						openTime = TimeUtil.ParseAnimeString(itemNode.CssSelect(".detail .date strong").First().InnerText);
					}
					yield return new Live { Id = id, Title = title, OpenTime = openTime, Thumbnail = thumbnail };
				}
			}
		}
	}
}
