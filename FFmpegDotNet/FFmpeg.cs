using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using FFmpegDotNet.Stream;

namespace FFmpegDotNet
{
	public sealed class FFmpeg : StreamGeneral
	{
		/// <summary>
		/// Set ffmpeg location
		/// </summary>
		string Exe { get { return Path.Combine("ffmpeg"); } }

		public FFmpeg(string filePath) : base(filePath)
		{

        }

		List<StreamVideo> _Video;
		public List<StreamVideo> Video
		{
			get
			{
				return null;
			}
		}

		List<StreamAudio> _Audio;
		public List<StreamAudio> Audio
		{
			get
			{
				return null;
			}
		}

		List<StreamSubtitle> _Subtitle;
		public List<StreamSubtitle> Subtitle
		{
			get
			{
				return null;
			}
		}
	}
}
