using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FFmpegDotNet
{
	public class StreamCommon
	{
		public string Id { get; internal set; }
		public string Language { get; internal set; }
		public string Codec { get; internal set; }
	}
}
