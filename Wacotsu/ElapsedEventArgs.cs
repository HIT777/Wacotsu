using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wacotsu
{
	public class ElapsedEventArgs
	{
		public TimeSpan leftTime { get; set; }

		public string LiveId { get; set; }
	}
}
