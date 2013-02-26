using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wacotsu
{
	public class ElapsedEventArgs : LiveEventArgs
	{
		public TimeSpan leftTime { get; set; }
	}
}
