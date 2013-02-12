using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiconicoApi.Live
{
	/// <summary>
	/// ニコニコ生放送の放送状態を表す
	/// </summary>
	public class Status
	{
		/// <summary>
		/// 部屋名
		/// </summary>
		public string RoomName { get; set; }

		/// <summary>
		/// 座席番号
		/// </summary>
		public int SeatNumber { get; set; }

		public override string ToString()
		{
			return string.Format("{0} - {1}", RoomName, SeatNumber);
		}
	}
}
