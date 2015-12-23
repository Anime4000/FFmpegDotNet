using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace FFmpegDotNet
{
	internal class Run
	{
		internal Run(string inFile, string outFile, string args)
		{
			var p = new Process();

			p.StartInfo = new ProcessStartInfo(OS.Terminal, $"{exe} -i {inFile} -y {outFile} {args}")
			{
				UseShellExecute = false,
				CreateNoWindow = true,
			};

			p.Start();
			p.WaitForExit();
		}
	}
}
