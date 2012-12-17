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
		/// 
		/// </summary>
		private IDictionary<Niconico.Live, Timer> timers;

		/// <summary>
		/// 
		/// </summary>
		private ICollection<Niconico.Live> gotLives;

		/// <summary>
		/// 
		/// </summary>
		private Niconico.User user;

		/// <summary>
		/// 
		/// </summary>
		public Wacotsu(Niconico.User user)
		{
			this.timers = new Dictionary<Niconico.Live, Timer>();
			this.gotLives = new List<Niconico.Live>();
			this.user = user;
		}

		/// <summary>
		/// 座席確保の予約をする
		/// </summary>
		/// <param name="live"></param>
		public void Reserve(Niconico.Live live)
		{
			if (this.timers.ContainsKey(live))
			{
				throw new Exception("すでに予約されている放送を予約することはできません");
			}
			if (this.gotLives.Contains(live))
			{
				throw new Exception("すでに座席確保した放送を予約することはできません");
			}
			var leftTime = live.OpenTime - DateTime.Now;
			// 開場時刻まであと10秒以内ならすぐに席取り開始
			if (leftTime.TotalMilliseconds < 10000)
			{
				this.getSeat(live);
				return;
			}
			if (leftTime.TotalMilliseconds - 10000 > Int32.MaxValue)
			{
				throw new Exception("開場時刻までの時間が遠すぎるため予約することができません");
			}
			var timer = new Timer(leftTime.TotalMilliseconds - 10000);
			timer.AutoReset = false;
			timer.Elapsed += (sender, args) => Task.Run(() => this.getSeat(live));
			this.timers.Add(live, timer);
			// 最初に一度確保を試みてエラーがないかチェック
			this.getSeat(live, false);
			// エラーがあって、すでに予約リストから削除されていたらここで中断
			if (this.timers.ContainsKey(live) == false)
			{
				return;
			}
			timer.Start();
		}

		/// <summary>
		/// 座席確保の予約を取り消す
		/// </summary>
		/// <param name="live"></param>
		public void Cancel(Niconico.Live live)
		{
			if (this.timers.ContainsKey(live) == false)
			{
				return;
			}
			this.timers[live].Stop();
			this.timers.Remove(live);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="live"></param>
		/// <param name="continuosly">まだ開場していなかった場合繰り返し取得を繰り返すかどうか</param>
		private void getSeat(Niconico.Live live, bool continuosly = true)
		{
			var getter = new Niconico.SeatGetter(this.user);
			try
			{
				while (true)
				{
					var seat = getter.GetSeat(live);
					if (seat == null)
					{
						if (continuosly)
						{
							continue;
						}
						else
						{
							return;
						}
					}
					this.gotLives.Add(live);
					this.Success(this, new SuccessEventArgs { Live = live, Seat = seat });
					break;
				}
			}
			catch (SeatFailedException ex)
			{
				FailedReason reason;
				switch (ex.ErrorCode)
				{
				case "notfound":
					reason = FailedReason.NotFound;
					break;

				case "notlogin":
					reason = FailedReason.NotLogin;
					break;

				case "noauth":
					reason = FailedReason.NoAuth;
					break;

				case "closed":
					reason = FailedReason.Closed;
					break;

				case "require_community_member":
					reason = FailedReason.RequireCommunityMember;
					break;

				default:
					throw new Exception("放送情報取得エラー code: " + ex.ErrorCode);
				}
				this.Failed(this, new FailedEventArgs { Live = live, Reason = reason });
				this.timers.Remove(live);
			}
			finally
			{
				getter.Dispose();
			}
		}
	}
}
