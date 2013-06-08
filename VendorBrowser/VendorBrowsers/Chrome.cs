using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Data.SQLite;
using Dapper;

namespace VendorBrowser.VendorBrowsers
{
	public class Chrome : VendorBrowser
	{
		public override void Open(Uri uri)
		{
			System.Diagnostics.Process.Start(this.GetExeFilePath(), uri.ToString());
		}

		public override Cookie GetCookie(string domain, string name)
		{
			var cookieFilePath = Path.Combine(GetUserDataDirectoryPath(), "Cookies");
			var connString = string.Format(@"Data Source={0}", cookieFilePath);
			using (var conn = new SQLiteConnection(connString)) {
				conn.Open();
				var query = string.Format("select * from cookies where host_key like '.{0}' and name like '{1}' limit 1", domain, name);
				var row = conn.Query(query).FirstOrDefault();
				if (row == null) {
					return null;
				}
				var cookie = new Cookie(row.name, row.value, row.path, row.host_key);
				conn.Close();
				return cookie;
			}
		}

		public override void SetCookie(string domain, Cookie cookie)
		{
			var insertQuery = string.Format("insert into cookies values({0}, '.{1}', '{2}', '{3}', '/', {4}, 0, 0, {5}, 1, 1",
				DateTime.UtcNow.ToFileTimeUtc(),
				domain,
				cookie.Name,
				cookie.Value,
				DateTime.UtcNow.AddYears(1).ToFileTimeUtc(),
				DateTime.UtcNow.ToFileTimeUtc()
			);
			var updateQuery = string.Format("update cookies set `value` = '{0}' where host_key like '.{1}' and name like '{2}';",
				cookie.Value, domain, cookie.Name);

			var query = (GetCookie(domain, cookie.Name) == null) ? insertQuery : updateQuery;

			var cookieFilePath = Path.Combine(GetUserDataDirectoryPath(), "Cookies");
			using (var conn = new SQLiteConnection(string.Format(@"Data Source={0}", cookieFilePath))) {
				conn.Open();
				conn.Query(query);
				conn.Close();
			}
		}

		protected override string GetExeFilePath()
		{
			var chromeRegPath = @"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\chrome.exe";
			var regKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(chromeRegPath);
			if (regKey == null) {
				return null;
			}
			using (regKey) {
				return Path.Combine(regKey.GetValue("Path").ToString(), "chrome.exe");
			}
		}

		protected override string GetUserDataDirectoryPath()
		{
			var localAppDirectoryPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
			return Path.Combine(localAppDirectoryPath, @"Google\Chrome\User Data\Default");
		}
	}
}
