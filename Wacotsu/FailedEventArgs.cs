using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wacotsu
{
	public class FailedEventArgs : EventArgs
	{
		public Live Live { get; set; }
		public Status Status { get; set; }
	}
}
