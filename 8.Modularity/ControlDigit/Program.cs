﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SRP.ControlDigit
{
	class Program
	{
		static void Main(string[] args)
		{
			var a = ControlDigitAlgo.Isbn10(6);
			Console.WriteLine(a);
		}
	}
}
