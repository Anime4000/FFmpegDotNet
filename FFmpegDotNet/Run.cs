using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace FFmpegDotNet
{
	internal class Run
	{

		internal int Execute(string args, string workDir)
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
				WorkingDirectory = workDir,
				UseShellExecute = false,
				CreateNoWindow = true,
			};

			p.Start();
			p.WaitForExit();

			return p.ExitCode;
		}
	}
}
