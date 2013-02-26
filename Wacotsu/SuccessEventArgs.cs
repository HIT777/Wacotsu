using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wacotsu
{
	/// <summary>
	/// Wacotsuが座席確保に成功した時のイベント情報
	/// </summary>
	public class SuccessEventArgs : LiveEventArgs
	{
		/// <summary>
		/// 確保した放送の状態
		/// </summary>
		public NiconicoApi.Live.Status LiveStatus { get; set; }
	}
}
