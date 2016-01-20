using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FFmpegDotNet
{
	public class StreamAttachment
	{
		public int Id { get; internal set; }
		public string FileName { get; internal set; }
		public string MimeType { get; internal set; }
	}
}
