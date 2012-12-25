using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wacotsu
{
	public enum Status 
	{
		/// <summary>
		/// 成功
		/// </summary>
		Ok,

		/// <summary>
		/// 放送前
		/// </summary>
		ComingSoon,

		/// <summary>
		/// 放送が見つからない
		/// </summary>
		NotFound,

		/// <summary>
		/// ログインしていない
		/// </summary>
		NotLogin,

		/// <summary>
		/// 特別な認証が必要であり、それを満たしていない
		/// </summary>
		NoAuth,

		/// <summary>
		/// すでに終了した放送である
		/// </summary>
		Closed,

		/// <summary>
		/// コミュニティに入会する必要がある
		/// </summary>
		RequireCommunityMember, 
	}
}
