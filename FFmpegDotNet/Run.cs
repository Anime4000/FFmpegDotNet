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
			var c = string.Empty;
			var a = string.Empty;
			var s = $"{FFmpeg.Bin} -i \"{inFile}\" -y \"{outFile}\" {args}";

			if (OS.IsWindows)
			{
				c = "cmd";
				a = $"/c {s}";
			}
			else
			{
				c = "sh";
				a = $"-c '{s}'";
			}

			p.StartInfo = new ProcessStartInfo(c, a)
			{
				UseShellExecute = false,
				CreateNoWindow = true,
			};
			
			p.Start();
			p.WaitForExit();
		}
	}
}
