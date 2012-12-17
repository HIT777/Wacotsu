using System;
using System.Collections.Generic;
using System.Text;
using System.Security.AccessControl;
using System.IO;
using System.Security.Principal;

namespace Td.Additional.IO
{
	/// <summary>
	/// File facilities.
	/// </summary>
	public static class File
	{
		/// <summary>
		/// Determines whether the specified file is readable.
		/// </summary>
		/// <param name="filename">The filename.</param>
		/// <returns>
		///   <c>true</c> if the specified file is readable; otherwise, <c>false</c>.
		/// </returns>
		public static bool IsReadable(string filename)
		{
			WindowsIdentity principal = WindowsIdentity.GetCurrent();
			if (System.IO.File.Exists(filename))
			{
				FileInfo fi = new FileInfo(filename);
				AuthorizationRuleCollection acl =
					fi.GetAccessControl().GetAccessRules(true, true, typeof(SecurityIdentifier));
				for (int i = 0; i < acl.Count; i++)
				{
					System.Security.AccessControl.FileSystemAccessRule rule = (System.Security.AccessControl.FileSystemAccessRule)acl[i];
					if (principal.User.Equals(rule.IdentityReference))
					{
						if (System.Security.AccessControl.AccessControlType.Deny.Equals
						(rule.AccessControlType))
						{
							if ((((int)FileSystemRights.Read) & (int)rule.FileSystemRights) == (int)(FileSystemRights.Read))
								return false;
						}
						else if (System.Security.AccessControl.AccessControlType.Allow.Equals
						(rule.AccessControlType))
						{
							if ((((int)FileSystemRights.Read) & (int)rule.FileSystemRights) == (int)(FileSystemRights.Read))
								return true;
						}
					}
				}
			}
			else
			{
				return false;
			}
			return false;
		}

		/// <summary>
		/// Determines whether the specified file is writeable.
		/// </summary>
		/// <param name="filename">The filename.</param>
		/// <returns>
		///   <c>true</c> if the specified file is writeable; otherwise, <c>false</c>.
		/// </returns>
		public static bool IsWriteable(string filename)
		{
			WindowsIdentity principal = WindowsIdentity.GetCurrent();
			if (System.IO.File.Exists(filename))
			{
				FileInfo fi = new FileInfo(filename);
				if (fi.IsReadOnly)
					return false;
				AuthorizationRuleCollection acl =
					fi.GetAccessControl().GetAccessRules(true, true, typeof(SecurityIdentifier));
				for (int i = 0; i < acl.Count; i++)
				{
					System.Security.AccessControl.FileSystemAccessRule rule = (System.Security.AccessControl.FileSystemAccessRule)acl[i];
					if (principal.User.Equals(rule.IdentityReference))
					{
						if (System.Security.AccessControl.AccessControlType.Deny.Equals
						(rule.AccessControlType))
						{
							if ((((int)FileSystemRights.Write) & (int)rule.FileSystemRights) == (int)(FileSystemRights.Write))
								return false;
						}
						else if (System.Security.AccessControl.AccessControlType.Allow.Equals
						(rule.AccessControlType))
						{
							if ((((int)FileSystemRights.Write) & (int)rule.FileSystemRights) == (int)(FileSystemRights.Write))
								return true;
						}
					}
				}
			}
			else
			{
				return false;
			}
			return false;
		}

	}
}