using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FFmpegDotNet.Stream
{
	public class Audio : General
	{
		public int SampleRate { get; protected set; }
		public int Channel { get; protected set; }
		public int BitDepth { get; protected set; }
	}
}
