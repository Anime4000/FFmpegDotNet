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

		public class Stream : Get
		{
			public Stream(string filePath) : base(filePath)
			{

			}
		}

		public class Process
		{
			public string[] Print(string filePath)
			{
				var f = Path.Combine(Path.GetTempPath(), $"{new Random().Next(0, 999999):D6}.imouto");
				new Run(filePath, string.Empty, $"2> {f}");
				return File.ReadAllLines(f);
			}
		}
	}
}
