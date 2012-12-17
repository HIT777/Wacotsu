using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wacotsu.Niconico
{
	/// <summary>
	/// 生放送の座席を表すクラス
	/// </summary>
	public class Seat
	{
		/// <summary>
		/// 座席エリア
		/// </summary>
		public string Label { get; set; }

		/// <summary>
		/// 座席番号
		/// </summary>
		public int Number { get; set; }

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return string.Format("{0} - {1}", this.Label, this.Number);
		}
	}
}
