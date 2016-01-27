using System;
using System.Diagnostics;

namespace FFmpegDotNet
{
	internal class Run
	{
		internal static string errorString { get; set; }

		internal int Execute(string args, string workDir)
		{
			var p = new Process();
			var c = string.Empty;
			var a = string.Empty;

			errorString = string.Empty; // make sure no old text holding

			Environment.SetEnvironmentVariable("FFMPEGDOTNET", args, EnvironmentVariableTarget.Process);

			if (OS.IsWindows)
			{
				c = "cmd";
				a = $"/c %FFMPEGDOTNET%";
            }
			else
			{
				c = "eval";
				a = "$FFMPEGDOTNET";
			}

			p.StartInfo = new ProcessStartInfo(c, a)
			{
				WorkingDirectory = workDir,
				UseShellExecute = false,
				CreateNoWindow = true,

				RedirectStandardError = true,
			};

			p.ErrorDataReceived += new DataReceivedEventHandler(consoleErrorHandler);

			p.Start();

			p.BeginErrorReadLine();

			p.WaitForExit();

			return p.ExitCode;
		}

		private void consoleErrorHandler(object sendingProcess, DataReceivedEventArgs errLine)
		{
			errorString += $"{errLine.Data}\n";
        }
	}
}