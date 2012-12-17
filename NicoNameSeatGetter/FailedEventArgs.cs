using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wacotsu
{
	public class FailedEventArgs : EventArgs
	{
		public Niconico.Live Live { get; set; }

		public FailedReason Reason { get; set; }
	}
}
