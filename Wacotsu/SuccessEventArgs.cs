using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wacotsu
{
	public class SuccessEventArgs : EventArgs
	{
		public Live Live { get; set; }
		public Seat Seat { get; set; }
	}
}
