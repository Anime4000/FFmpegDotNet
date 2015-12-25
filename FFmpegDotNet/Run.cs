using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace FFmpegDotNet
{
	internal class Run
	{
		internal Run(string inFile, string probeFile)
		{
			Execute($"\"{FFmpeg.Probe}\" -print_format xml -show_format -show_streams \"{inFile}\" > \"{probeFile}\"");
        }

		internal Run(string inFile, string outFile, string args)
		{
			Execute($"\"{FFmpeg.Bin}\" -i \"{inFile}\" -y \"{outFile}\" {args}");
		}

		private int Execute(string args)
		{
			var p = new Process();
			var c = string.Empty;
			var a = string.Empty;

			Environment.SetEnvironmentVariable("FFMPEGDOTNET", args, EnvironmentVariableTarget.Process);

			if (OS.IsWindows)
			{
				c = "cmd";
				a = $"/c %FFMPEGDOTNET%";
            }
			else
			{
				c = "sh";
				a = $"-c '{args}'";
			}

			p.StartInfo = new ProcessStartInfo(c, a)
			{
				UseShellExecute = false,
				CreateNoWindow = true,
			};

			p.Start();
			p.WaitForExit();

			return p.ExitCode;
		}
	}
}
