using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Diagnostics;

namespace Wacotsu
{
	/// <summary>
	/// Wacotsu本体
	/// </summary>
	public sealed class Wacotsu
	{
		/// <summary>
		/// サーバー時刻を同期する間隔時間（秒）
		/// </summary>
		private const int ServerTimeSyncIntervalSecond = 30;

		/// <summary>
		/// 座席確保に成功した時
		/// </summary>
		public event EventHandler<SuccessEventArgs> Success = delegate { };

		/// <summary>
		/// 座席確保に失敗した時
		/// </summary>
		public event EventHandler<FailedEventArgs> Failed = delegate { };

		/// <summary>
		/// ニコニコapi
		/// </summary>
		private NiconicoApi.NiconicoApi api;

		/// <summary>
		/// 予約確定リスト
		/// </summary>
		private List<LiveOpenInfo> reservedLives;

		/// <summary>
		/// 予約待ち行列
		/// </summary>
		private ConcurrentQueue<LiveOpenInfo> reserveQueue;

		/// <summary>
		/// キャンセル待ち行列
		/// </summary>
		private ConcurrentQueue<string> cancelQueue;

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public Wacotsu(NiconicoApi.NiconicoApi api)
		{
			this.api = api;
			this.reservedLives = new List<LiveOpenInfo>();
			this.reserveQueue = new ConcurrentQueue<LiveOpenInfo>();
			this.cancelQueue = new ConcurrentQueue<string>();
			Task.Run(() => runBackgroundWork());
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

			// 最初の一度ですでにキャンセル待ちになっていた場合は
			// 確保成功済み　または　生放送エラーなので予約しない
			if (cancelQueue.Contains(liveId)) {
				return;
			}

			reserveQueue.Enqueue(new LiveOpenInfo { LiveId = liveId, OpenTime = openTime });
		}

		/// <summary>
		/// 予約した放送をキャンセルする
		/// </summary>
		/// <param name="liveId"></param>
		public void Cancel(string liveId)
		{
			cancelQueue.Enqueue(liveId);
		}

		private void runBackgroundWork()
		{
			var serverTime = api.GetServerTime();
			DateTime timeCounter = DateTime.Now;
			while (true) {
				var span = DateTime.Now - timeCounter;
				if (span.TotalSeconds >= ServerTimeSyncIntervalSecond) {
					serverTime = api.GetServerTime();
					timeCounter = DateTime.Now;
					Debug.WriteLine("update server time: {0}", serverTime);
				}
				backgroundWork(serverTime);
				System.Threading.Thread.Sleep(100);
				serverTime = serverTime.AddMilliseconds(100);
			}
		}

		private void backgroundWork(DateTime serverTime)
		{
			// 予約追加を確認
			LiveOpenInfo reserveLiveInfo;
			if (reserveQueue.TryDequeue(out reserveLiveInfo)) {
				reservedLives.Add(reserveLiveInfo);
				Debug.WriteLine("add reserve: {0}", reserveLiveInfo.LiveId);
			}

			// 予約削除を確認
			string cancelLiveId;
			if (cancelQueue.TryDequeue(out cancelLiveId)) {
				reservedLives.RemoveAll((l) => l.LiveId == cancelLiveId);
				Debug.WriteLine("cancel reserve: {0}", cancelLiveId);
			}

			// 残り時間をループで確認
			foreach (var liveInfo in reservedLives) {
				// 残り時間わずかなら取得
				var leftTimeSpan = liveInfo.OpenTime - serverTime;
				if (leftTimeSpan.TotalMilliseconds <= 2000) {
					fetch(liveInfo.LiveId);
					Debug.WriteLine("fetch live!: {0}", liveInfo.LiveId);
				}
			}
		}

		/// <summary>
		/// 放送の座席を取得
		/// </summary>
		/// <param name="liveId"></param>
		private void fetch(string liveId)
		{
			try {
				// アクセス
				var status = api.GetLiveStatus(liveId);
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
				// それ以外のエラーは大抵503なのでそのまま再試行
			}
		}
	}
}
