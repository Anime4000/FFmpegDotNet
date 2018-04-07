using System.Diagnostics;

namespace FFmpegDotNet
{
    internal class Run
    {
        internal string Output { get; set; } = string.Empty;

        internal Run(string FileMedia)
        {
            var p = new Process();

            var exe = FFmpeg.FFmpegProbe;
            var arg = $"-hide_banner -print_format json -show_format -show_streams \"{FileMedia}\"";

            if (OS.IsWindows) exe += ".exe";

            p.StartInfo = new ProcessStartInfo(exe, arg)
            {
                UseShellExecute = false,
                CreateNoWindow = true,

                RedirectStandardOutput = true
            };

            p.OutputDataReceived += new DataReceivedEventHandler(ConsoleStandardHandler);

            p.Start();

            p.BeginOutputReadLine();

            p.WaitForExit();
        }

        private void ConsoleStandardHandler(object s, DataReceivedEventArgs e)
        {
            Output += e.Data;
        }
    }
}