using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wacotsu
{
	public class SeatFailedException : Exception
	{
		public string ErrorCode { get; set; }
	}
}
