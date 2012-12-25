using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace timertest
{
	class Program
	{
		static void Main(string[] args)
		{
			var timer = new Timer();
			timer.Interval = 1;
			timer.Elapsed += (sender, e) => Console.WriteLine("elapsed.");
			timer.AutoReset = true;
			timer.Start();
			Console.ReadLine();
		}
	}
}
