﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wacotsu
{
	public class FailedEventArgs : LiveEventArgs
	{
		public NiconicoApi.Live.StatusErrorReason FailReason { get; set; }
	}
}
