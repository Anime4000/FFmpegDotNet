using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FFmpegDotNet.Stream
{
	public class Get
	{
		public Get(string filePath)
		{
			foreach (var item in FFmpeg.Process.Print(filePath))
			{
				
			}
		}
	}
}
