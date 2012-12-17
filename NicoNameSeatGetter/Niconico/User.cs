using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wacotsu.Niconico
{
	/// <summary>
	/// ニコニコ動画のログイン中ユーザー情報
	/// </summary>
	public class User
	{
		/// <summary>
		/// クッキーに保存されている認証セッションID
		/// このセッションをクッキーとして別のブラウザと共有することでログイン状態を共有できる
		/// </summary>
		public string SessionId { get; set; }
	}
}