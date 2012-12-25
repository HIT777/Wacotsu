using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Wacotsu
{
	/// <summary>
	/// Wacotsu本体
	/// </summary>
	public sealed class Wacotsu
	{
		/// <summary>
		/// 座席確保に成功した時
		/// </summary>
		public event EventHandler<SuccessEventArgs> Success = delegate { };

		/// <summary>
		/// 座席確保に失敗した時
		/// </summary>
		public event EventHandler<FailedEventArgs> Failed = delegate { };

		/// <summary>
		/// 予約中の放送 -> 座席取得タイマーへのマップ 
		/// </summary>
		private IDictionary<Live, Timer> timers = new Dictionary<Live, Timer>();

		/// <summary>
		/// 予約した放送 
		/// </summary>
		private ICollection<Live> reserved = new List<Live>();

		/// <summary>
		/// 座席確保に成功した放送
		/// </summary>
		private ICollection<Live> finished = new List<Live>();

		/// <summary>
		/// 
		/// </summary>
		private SeatFetcher seatFetcher;

		/// <summary>
		/// 
		/// </summary>
		public Wacotsu(string userSession)
		{
			this.seatFetcher = new SeatFetcher(userSession);
		}

		/// <summary>
		/// 座席確保の予約をする
		/// </summary>
		/// <param name="live"></param>
		public bool Reserve(Live live)
		{
			// すでに予約済・確保済ならば中断
			if (this.reserved.Contains(live) ||
				this.finished.Contains(live))
			{
				return false;
			}

			var timer = new Timer();
			timer.Elapsed += (sender, args) =>
			{
				var result = this.seatFetcher.Fetch(live);
				if (result.Status == Status.Ok)
				{
					this.finished.Add(live);
					this.Success(this, new SuccessEventArgs { Live = live, Seat = result.Seat });
					this.Cancel(live);
					return;
				}
				else if(result.Status != Status.ComingSoon)
				{
					this.Failed(this, new FailedEventArgs { Live = live, Status = result.Status });
					this.Cancel(live);
					return;
				}
				// 残り時間を取得
				var leftSpan = live.OpenTime - result.Time;
				if (leftSpan.TotalSeconds <= 5)
				{
					timer.Interval = 1;
				}
				else
				{
					timer.Interval = leftSpan.TotalMilliseconds - 5000;	
				}
				timer.Start();
			};
			timer.AutoReset = false;
			timer.Interval = 1;
			timer.Start();
			this.timers.Add(live, timer);
			return true;
		}

		/// <summary>
		/// 座席確保の予約を取り消す
		/// </summary>
		/// <param name="live"></param>
		public void Cancel(Live live)
		{
			if (this.timers.ContainsKey(live) == false)
			{
				return;
			}
			this.timers[live].Stop();
			this.timers.Remove(live);
		}

	}
}
