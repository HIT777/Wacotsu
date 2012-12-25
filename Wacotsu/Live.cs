using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Wacotsu
{
	/// <summary>
	/// ニコニコ生放送の放送を表すクラス
	/// </summary>
	public class Live
	{
		/// <summary>
		/// 放送ID
		/// </summary>
		public string Id { get; set; }

		/// <summary>
		/// 放送のタイトル
		/// </summary>
		public string Title { get; set; }

		/// <summary>
		/// サムネイル画像
		/// </summary>
		public Image Thumbnail { get; set; } 

		/// <summary>
		/// 開場時刻
		/// </summary>
		public DateTime OpenTime { get; set; }

		/// <summary>
		/// 放送視聴ページのURI
		/// </summary>
		public Uri WatchUri { get { return new Uri(string.Format("http://live.nicovideo.jp/watch/{0}?ref=grel", this.Id)); } }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="span"></param>
		/// <returns></returns>
		public string GetLeftTimeString()
		{
			var span = this.OpenTime - DateTime.Now;
			var result = new StringBuilder("あと");
			if (span.Days > 0)
			{
				result.Append(string.Format("{0}日", span.Days));
			}
			if (span.Hours > 0)
			{
				result.Append(string.Format("{0}時間", span.Hours));
			}
			if (span.Minutes > 0)
			{
				result.Append(string.Format("{0}分", span.Minutes));
			}
			if (span.TotalMinutes < 1)
			{
				if (span.TotalSeconds <= 0)
				{
					result.Clear().Append("上映中");
				}
				else
				{
					result.Clear().Append("もうすぐ開場");
				}
			}
			return result.ToString();
		}
	}
}
