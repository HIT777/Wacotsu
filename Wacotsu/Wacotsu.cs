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
		/// 予約中の放送ID -> 開場時間のマップ
		/// </summary>
		private IDictionary<string, DateTime> reservedLiveTimes;

		/// <summary>
		/// 座席取得中の放送ID　一覧
		/// </summary>
		private ICollection<string> fetchLives;

		/// <summary>
		/// ニコニコapi
		/// </summary>
		private NiconicoApi.NiconicoApi api;

		/// <summary>
		/// サーバー時刻
		/// </summary>
		private DateTime serverTime;

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public Wacotsu(NiconicoApi.NiconicoApi api)
		{
			this.api = api;
			this.reservedLiveTimes = new ConcurrentDictionary<string, DateTime>();
			this.fetchLives = new List<string>();

			// サーバー時間合わせスレッド
			Task.Run(() => {
				while (true) {
					serverTime = api.GetServerTimeAsync().Result;
					System.Threading.Thread.Sleep(10000);
				}
			});

			// 残り時間確認スレッド
			Task.Run(() => checkReserved());

			// 座席取得スレッド
			Task.Run(() => {
				while (true) {
					foreach (var liveId in fetchLives) {
						fetch(liveId);
					}
					if (fetchLives.Count < 1) {
						System.Threading.Thread.Sleep(10);
					}
				}
			});
		}

		/// <summary>
		/// 放送を予約する
		/// </summary>
		/// <param name="liveId">放送のID</param>
		/// <param name="openTime">放送の開場時刻</param>
		public void Reserve(string liveId, DateTime openTime)
		{
			// エラー確認のため最初に一度取得を試みる
			fetch(liveId);

			// 放送開場時間までの残り時間を取得
			reservedLiveTimes.Add(liveId, openTime);
		}

		/// <summary>
		/// 予約した放送をキャンセルする
		/// </summary>
		/// <param name="liveId"></param>
		public void Cancel(string liveId)
		{
			if (reservedLiveTimes.ContainsKey(liveId)) {
				reservedLiveTimes.Remove(liveId);
			}

			if (fetchLives.Contains(liveId)) {
				lock (((System.Collections.ICollection)fetchLives).SyncRoot) {
					fetchLives.Remove(liveId);
				}
			}
		}

		/// <summary>
		/// 予約中の放送開始までの残り時間を監視して
		/// 残りわずかになったら取得リストに移す
		/// </summary>
		private void checkReserved()
		{
			while (true) {
				foreach (var pair in reservedLiveTimes) {
					var liveId = pair.Key;
					var openTime = pair.Value;
					// 残り時間を出す
					var leftTimeSpan = openTime - serverTime;
					// 残り時間がわずかなら取得リストに移す
					if (leftTimeSpan.TotalMilliseconds <= 1000) {
						lock (((System.Collections.ICollection)fetchLives).SyncRoot) {
							fetchLives.Add(liveId);
						}
						reservedLiveTimes.Remove(liveId);
						break;
					}
				}
				System.Threading.Thread.Sleep(10);
			}
		}

		/// <summary>
		/// 放送の座席を取得
		/// </summary>
		/// <param name="liveId"></param>
		private async void fetch(string liveId)
		{
			try {
				// アクセス
				var status = await api.GetLiveStatusAsync(liveId);
				if (status != null) {
					var successArgs = new SuccessEventArgs { LiveId = liveId, LiveStatus = status };
					this.Success(this, successArgs);
					Cancel(liveId);
				}
			}
			catch (Exception ex) {
				// エラーがニコニコ生放送のエラーならこれ以上再試行しても無駄なので中止する
				if (ex is NiconicoApi.Live.StatusException) {
					var errorReason = ((NiconicoApi.Live.StatusException)ex).Reason;
					var failedArgs = new FailedEventArgs { LiveId = liveId, FailReason = errorReason };
					this.Failed(this, failedArgs);
					Cancel(liveId);
				}
			}
		}
	}
}
