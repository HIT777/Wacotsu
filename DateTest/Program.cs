using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DateTest
{
	class Program
	{
		static void Main(string[] args)
		{
			var d = DateTime.ParseExact("2012/12/22", "yyyy/MM/dd", System.Globalization.CultureInfo.InvariantCulture);
			Console.WriteLine(d);
		}
	}
}
