using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Globalization;

namespace Wacotsu
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
		public static DateTime UnixTimeToDateTime(string text)
		{
			double seconds = double.Parse(text, System.Globalization.CultureInfo.InvariantCulture);
			return Epoch.AddSeconds(seconds);
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
