using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiconicoApi.Live
{
	/// <summary>
	/// 放送情報の取得失敗時に発生する例外
	/// </summary>
	public class StatusException : Exception
	{
		private StatusErrorReason reason;

		/// <summary>
		/// 失敗の理由
		/// </summary>
		public StatusErrorReason Reason { get { return reason; } }

		public StatusException(StatusErrorReason reason)
		{
			this.reason = reason;	
		}

		public StatusException(string reasonString)
		{
			switch (reasonString) {
			case "notfound":
				this.reason = StatusErrorReason.NotFound;
				break;

			case "notlogin":
				this.reason = StatusErrorReason.NotLogin;
				break;

			case "noauth":
				this.reason = StatusErrorReason.NoAuth;
				break;

			case "closed":
				this.reason = StatusErrorReason.Closed;
				break;

			case "require_community_member":
				this.reason = StatusErrorReason.RequireCommunityMember;
				break;

			default:
				this.reason = StatusErrorReason.Unknown;
				break;
			}	
		}
	}
}
