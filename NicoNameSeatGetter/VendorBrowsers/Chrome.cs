using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data.SQLite;

namespace Wacotsu.VendorBrowsers
{
	public class Chrome : VendorBrowser
	{
		public override void Open(Uri uri)
		{
			System.Diagnostics.Process.Start(this.GetExeFilePath(), uri.ToString());
		}

		public override System.Net.Cookie GetCookie(Uri uri, string name)
		{
			var cookieFilePath = Path.Combine(GetUserDataDirectoryPath(), "Cookies");
			if (Td.Additional.IO.File.IsReadable(cookieFilePath) == false)
			{
				throw new Exception("ブラウザのクッキー情報を取得できません。\r\n一度ブラウザを閉じてください");
			}
			var connString = string.Format(@"Data Source={0};pooling=false;mode=ro;Read Only=True;", cookieFilePath);
			using (var conn = new SQLiteConnection(connString))
			using (var command = conn.CreateCommand())
			{
				command.CommandText = string.Format("select * from cookies where host_key like '.{0}' and name like '{1}' limit 1", uri.Host, name);
				conn.Open();
				using (var reader = command.ExecuteReader())
				{
					var values = reader.GetValues();
					if (values == null)
					{
						return null;
					}
					var cookie = new System.Net.Cookie(values["name"], values["value"], values["path"], values["host_key"]);
					conn.Close();
					return cookie;
				}
			}
		}

		public override void SetCookie(Uri uri, System.Net.Cookie cookie)
		{
			var commandText = "";
			if (GetCookie(uri, cookie.Name) != null)
			{
				commandText = string.Format("update cookies set `value` = '{0}' where host_key like '.{1}' and name like '{2}';",
					cookie.Value, uri.Host, cookie.Name);
			}
			else
			{
				commandText = string.Format("insert into cookies values({0}, '.{1}', '{2}', '{3}', '/', {4}, 0, 0, {5}, 1, 1",
					DateTime.UtcNow.ToFileTimeUtc(),
					uri.Host,
					cookie.Name,
					cookie.Value,
					DateTime.UtcNow.AddYears(1).ToFileTimeUtc(),
					DateTime.UtcNow.ToFileTimeUtc()
				);
			}

			var cookieFilePath = Path.Combine(GetUserDataDirectoryPath(), "Cookies");
			using (var conn = new SQLiteConnection(string.Format(@"Data Source={0}", cookieFilePath)))
			using (var command = conn.CreateCommand())
			{
				conn.Open();
				command.CommandText = commandText;
				command.ExecuteNonQuery();
				conn.Close();
			}
		}

		protected override string GetExeFilePath()
		{
			using (var regKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(
				@"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\chrome.exe"))
			{
				if (regKey == null)
				{
					return null;
				}
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
