using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Globalization;

namespace NiconicoApi 
{
	public static class TimeUtil
	{
		/// <summary>
		/// 
		/// </summary>
		private static readonly DateTime Epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="text"></param>
		/// <returns></returns>
		public static DateTime UnixTimeToDateTime(string unixTimeString)
		{
			double seconds = double.Parse(unixTimeString, CultureInfo.InvariantCulture);
			return Epoch.AddSeconds(seconds);
		}

		/// <summary>
		/// 時間差から「xx日yy時間zz分」形式の文字列を作成する
		/// </summary>
		/// <param name="span"></param>
		/// <returns></returns>
		public static string GetLeftTimeSpanString(TimeSpan span)
		{
			var sb = new StringBuilder();
			if (span.Days > 0) {
				sb.AppendFormat("{0}日", span.Days);
			}
			if (span.Hours > 0) {
				sb.AppendFormat("{0}時間", span.Hours);
			}
			if (span.Minutes > 0) {
				sb.AppendFormat("{0}分", span.Minutes);
			}
			return sb.ToString();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="source"></param>
		/// <returns></returns>
		public static DateTime ParseAnimeString(string source)
		{
			var sources = source.Split(' ');
			var baseDateTime = DateTime.Parse(sources[0], CultureInfo.InvariantCulture);
			var hour = int.Parse(sources[1].Split(':')[0]);
			var minute = int.Parse(sources[1].Split(':')[1]);
			var addedSpan = new TimeSpan(hour, minute, 0);
			return baseDateTime.Add(addedSpan);
		}
	}
}
