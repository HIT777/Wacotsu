using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Net;
using System.Drawing;
using HtmlAgilityPack;
using ScrapySharp.Extensions;

namespace Wacotsu.Niconico
{
	public static class LiveFetcher
	{
		private const string liveListFeedUri = "http://live.nicovideo.jp/rss";

		/// <summary>
		/// 直近一週間分の公式生放送一覧をRSSフィードから取得する
		/// </summary>
		/// <returns></returns>
		public static IEnumerable<Live> FetchLives(int count = 20)
		{
			using (var client = new Net.WebClientWithCookie())
			{
				client.Encoding = UTF8Encoding.UTF8;
				var xmlString = client.DownloadString(LiveFetcher.liveListFeedUri);
				var xml = XDocument.Parse(xmlString);
				var nsLive = xml.Root.GetNamespaceOfPrefix("nicolive");
				var nsMedia = xml.Root.GetNamespaceOfPrefix("media");
				var items = xml.Descendants("item").Take(count);
				foreach (var node in items)
				{
					var title = node.Descendants("title").First().Value;
					var id = node.Descendants("guid").First().Value;
					var openTime = DateTime.Parse(node.Descendants(nsLive + "open_time").First().Value);
					var startTime = DateTime.Parse(node.Descendants(nsLive + "start_time").First().Value);
					var thumbnailUri = new Uri(node.Descendants(nsMedia + "thumbnail").First().Attribute("url").Value);
					Image thumbnail = null;
					using (var stream = HttpWebRequest.Create(thumbnailUri).GetResponse().GetResponseStream())
					{
						thumbnail = Image.FromStream(stream);
					}
					yield return new Live { Id = id, OpenTime = openTime, Thumbnail = thumbnail, Title = title };
				}
			}
		}

		public static IEnumerable<Live> SearchLives()
		{
			using (var client = new Net.WebClientWithCookie())
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
						openTime = Time.TimeUtil.ParseAnimeString(itemNode.CssSelect(".detail .date strong").First().InnerText);
					}
					yield return new Live { Id = id, Title = title, OpenTime = openTime, Thumbnail = thumbnail };
				}
			}
		}

		private static string GetInputValue(HtmlNode form, string name)
		{
			return form.CssSelect(string.Format("input[name={0}]", name)).First().Attributes["value"].Value;
		}
	}
}
