using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FFmpegDotNet.Stream
{
	public class Video : General
	{
		public string PixelFormat { get; protected set; }
		public int Width { get; protected set; }
		public int Height { get; protected set; }
		public float FrameRate { get; protected set; }
	}
}
