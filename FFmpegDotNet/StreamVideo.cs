using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FFmpegDotNet
{
	public class StreamVideo : StreamCommon
	{
		public string PixelFormat { get; internal set; }
		public int BitPerColour { get; internal set; }
		public int Width { get; internal set; }
		public int Height { get; internal set; }
		public float FrameRate { get; internal set; }
		public int FrameCount { get; internal set; }
	}
}
