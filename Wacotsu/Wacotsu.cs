using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
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
		/// タイマーが開場までの残り時間をチェックするたびに発生するイベント
		/// </summary>
		public event EventHandler<ElapsedEventArgs> TimerElapsed = delegate { };

		/// <summary>
		/// サーバーへのアクセスを開始した時
		/// </summary>
		public event EventHandler<LiveEventArgs> StartAccess = delegate { };

		/// <summary>
		/// サーバーからの返事が返ってきた時
		/// </summary>
		public event EventHandler<LiveEventArgs> ReceiveResponse = delegate { };

		/// <summary>
		/// アクセスを再試行したとき
		/// </summary>
		public event EventHandler<LiveEventArgs> RetryAccess = delegate { };

		/// <summary>
		/// 予約中の放送ID -> 開場時間のマップ
		/// </summary>
		private IDictionary<string, DateTime> reservedLiveTimes;

		/// <summary>
		/// 
		/// </summary>
		private NiconicoApi.NiconicoApi api;

		/// <summary>
		/// 
		/// </summary>
		public Wacotsu(NiconicoApi.NiconicoApi api, int pollingMillisecond = 3000)
		{
			this.api = api;
			this.reservedLiveTimes = new ConcurrentDictionary<string, DateTime>();

			var timer = new Timer(pollingMillisecond);
			timer.Elapsed += (sender, e) => {
				if (reservedLiveTimes.Count < 1) {
					return;
				}

				var serverTime = api.GetServerTimeAsync().GetAwaiter().GetResult();

				foreach (var pair in reservedLiveTimes) {
					var liveId = pair.Key;
					var openTime = pair.Value;

					var leftTimeSpan = pair.Value - serverTime;
					this.TimerElapsed(this, new ElapsedEventArgs { leftTime = leftTimeSpan, LiveId = liveId });
					// 開場までの残り時間がわずかになったら席取得を開始する
					if (leftTimeSpan.TotalMilliseconds <= pollingMillisecond + 500) {
						reservedLiveTimes.Remove(liveId);
						getLiveStatusAsync(liveId);
					}
				}
			};
			timer.Start();
		}

		/// <summary>
		/// 放送を予約する
		/// </summary>
		/// <param name="liveId">放送のID</param>
		/// <param name="openTime">放送の開場時刻</param>
		public async void Reserve(string liveId, DateTime openTime)
		{
			// エラー確認のため最初に一度取得を試みる
			try {
				var status = await api.GetLiveStatusAsync(liveId);
			}
			catch (Exception e) {
				if (e is NiconicoApi.Live.StatusException) {
					var errorReason = (e as NiconicoApi.Live.StatusException).Reason;
					this.Failed(this, new FailedEventArgs { LiveId = liveId, FailReason = errorReason });
					return;
				}
				else {
					this.Failed(this, new FailedEventArgs { LiveId = liveId, FailReason = NiconicoApi.Live.StatusErrorReason.Unknown });
					return;
				}
			}

			// 放送開場時間までの残り時間を取得
			var serverTime = await api.GetServerTimeAsync();
			var leftTimeSpan = openTime - serverTime;
			if (leftTimeSpan.TotalMilliseconds <= 3000) {
				getLiveStatusAsync(liveId);
				return;
			}
			reservedLiveTimes.Add(liveId, openTime);
		}

		/// <summary>
		/// 予約した放送をキャンセルする
		/// </summary>
		/// <param name="liveId"></param>
		public void Cancel(string liveId)
		{
			if (reservedLiveTimes.ContainsKey(liveId) == false) {
				return;
			}

			reservedLiveTimes.Remove(liveId);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="liveId"></param>
		private async void getLiveStatusAsync(string liveId)
		{
			NiconicoApi.Live.Status status = null;
			var eventArgs = new LiveEventArgs { LiveId = liveId };
			try {
				var retryCounter = 0;
				while (status == null) {
					if (retryCounter > 0) {
						this.RetryAccess(this, eventArgs);
					}
					this.StartAccess(this, eventArgs);
					status = await api.GetLiveStatusAsync(liveId);
					this.ReceiveResponse(this, eventArgs);
					retryCounter++;
					// 20回以上再試行しても座席確保できない場合は原因不明のエラーを投げる
					if (retryCounter > 20) {
						throw new NiconicoApi.Live.StatusException(NiconicoApi.Live.StatusErrorReason.Unknown);
					}
				}
				this.Success(this, new SuccessEventArgs { LiveId = liveId, LiveStatus = status });
			}
			catch (Exception e) {
				if (e is NiconicoApi.Live.StatusException) {
					var reason = (e as NiconicoApi.Live.StatusException).Reason;
					this.Failed(this, new FailedEventArgs { LiveId = liveId, FailReason = reason });
				}
				else {
					this.Failed(this, new FailedEventArgs { LiveId = liveId, FailReason = NiconicoApi.Live.StatusErrorReason.Unknown });
				}
			}
			finally {
				// 成功・失敗に限らず予約は終わったので予約リストに残っていれば予約リストから削除
				if (reservedLiveTimes.ContainsKey(liveId)) {
					reservedLiveTimes.Remove(liveId);
				}
			}
		}

	}
}
