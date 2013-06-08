using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace VendorBrowser 
{
	/// <summary>
	/// 外部ブラウザを表すクラス
	/// </summary>
	public abstract class VendorBrowser
	{
		public static VendorBrowser GetDefaultBrowser()
		{
			using (var regKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(@"http\shell\open\command"))
			{
				var value = regKey.GetValue(null).ToString();
				if (value.Contains("chrome"))
				{
					return new VendorBrowsers.Chrome();
				}
				return null;
			}
		}

		/// <summary>
		/// 指定したURIをブラウザで開く
		/// </summary>
		/// <param name="uri"></param>
		public abstract void Open(Uri uri);

		/// <summary>
		/// クッキーを取得する
		/// </summary>
		/// <param name="domain"></param>
		/// <param name="name"></param>
		/// <returns></returns>
		public abstract Cookie GetCookie(string domain, string name);

		/// <summary>
		/// クッキーを設定する
		/// </summary>
		/// <param name="uri"></param>
		/// <param name="cookie"></param>
		public abstract void SetCookie(string domain, Cookie cookie);

		/// <summary>
		/// ブラウザの実行ファイルの完全パスを取得する
		/// </summary>
		/// <returns></returns>
		protected abstract string GetExeFilePath();

		/// <summary>
		/// ブラウザのユーザーデータが保存されているディレクトリの完全パスを取得する
		/// </summary>
		/// <returns></returns>
		protected abstract string GetUserDataDirectoryPath();
	}
}
