using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiconicoApi.Live
{
	/// <summary>
	/// ニコニコ生放送の放送情報を表すクラス
	/// </summary>
	public class Info 
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
		/// サムネイル画像のURI
		/// </summary>
		public Uri ImageUri { get; set; } 

		/// <summary>
		/// 開場時刻
		/// </summary>
		public DateTime OpenTime { get; set; }

		/// <summary>
		/// 放送視聴ページのURI
		/// </summary>
		public Uri WatchUri { get { return new Uri(string.Format("http://live.nicovideo.jp/watch/{0}?ref=grel", Id)); } }

		/// <summary>
		/// 放送ゲートページのURI
		/// </summary>
		public Uri GateUri { get { return new Uri(string.Format("http://live.nicovideo.jp/gate/{0}", Id)); } }
	}
}
