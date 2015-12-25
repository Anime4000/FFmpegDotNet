using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace FFmpegDotNet
{
	public class FFmpeg
	{
		public static string Bin { get; set; } = Path.Combine("ffmpeg");
		public static string Probe { get; set; } = Path.Combine("ffprobe");

		public class Stream : Get
		{
			public Stream(string filePath) : base(filePath)
			{

			}
		}

		public class Process
		{
			public string Print(string filePath)
			{
				var f = Path.Combine(Path.GetTempPath(), $"imouto-{new Random().Next(0, 999999):D6}.xml");
				new Run(filePath, f);
				return f;
			}
		}
	}
}
