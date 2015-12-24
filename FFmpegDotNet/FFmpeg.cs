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

		public class Stream
		{
			public Stream(string filePath)
			{

			}


		}

		public class Process
		{
			public string Print(string filePath)
			{
				return string.Empty;
			}
		}
	}
}
