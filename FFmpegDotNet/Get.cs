using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;

namespace FFmpegDotNet
{
	public class Get
	{
		StringComparison IC { get { return StringComparison.InvariantCultureIgnoreCase; } }

		public Get(string filePath)
		{
			var file = Path.Combine(Path.GetTempPath(), $"nemu_{new Random().Next(0, 999999999):D9}.xml");
			new Run().Execute($"\"{FFmpeg.Probe}\" -print_format xml -show_format -show_streams \"{filePath}\" > \"{file}\"", Path.GetTempPath());

			var xml = XDocument.Load(file);
			var format = from a in xml.Descendants("format")
						 select new
						 {
							 fmt = a.Attribute("format_name").Value,
							 fmtlong = a.Attribute("format_long_name").Value,
							 size = a.Attribute("size").Value,
							 bitrate = a.Attribute("bit_rate").Value,
							 duration = a.Attribute("duration").Value,
						 };

			var video = from a in xml.Descendants("stream")
						where string.Equals("video", (string)a.Attribute("codec_type"))
						select new
						{
							id = (int)a.Attribute("index"),
							lang = a.Element("tag").Attribute("value").Value,
							codec = a.Attribute("codec_name").Value,
							pixfmt = a.Attribute("pix_fmt").Value,
							bpc = a.Attribute("bits_per_raw_sample").Value,
							width = a.Attribute("width").Value,
							height = a.Attribute("height").Value,
							fps = a.Attribute("r_frame_rate").Value,
							framecount = a.Attribute("nb_frames")?.Value,
						};

			var audio = from a in xml.Descendants("stream")
						where string.Equals("audio", (string)a.Attribute("codec_type"))
						select new
						{
							id = (int)a.Attribute("index"),
							lang = a.Element("tag").Attribute("value").Value,
							codec = a.Attribute("codec_name").Value,
							sample = a.Attribute("sample_rate").Value,
							bitdepth = a.Attribute("sample_fmt").Value,
                            channel = a.Attribute("channels").Value,
						};

			var subtitle = from a in xml.Descendants("stream")
						   where string.Equals("subtitle", (string)a.Attribute("codec_type"))
						   select new
						   {
							   id = (int)a.Attribute("index"),
							   lang = a.Element("tag").Attribute("value").Value,
							   codec = a.Attribute("codec_name").Value,
						   };

			var attachment = from a in xml.Descendants("stream")
							 where string.Equals("attachment", (string)a.Attribute("codec_type"))
							 select new
							 {
								 id = (int)a.Attribute("index"),
								 filename = a.Element("tag")?.Attribute("value").Value,
								 mimetype = string.Empty, // how to get second element under this block?

							 };

			foreach (var item in format)
			{
				FormatName = item.fmt;
				FormatNameFull = item.fmtlong;
				FileSize = ulong.Parse(item.size);
				BitRate = ulong.Parse(item.bitrate);
				Duration = float.Parse(item.duration);

				break; // single only
			}

			foreach (var item in video)
			{
				int bpc = 8;
				int w = 0;
				int h = 0;
				int fc = 0;
				float num = 0;
				float den = 0;

				int.TryParse(item.bpc, out bpc);
				int.TryParse(item.width, out w);
				int.TryParse(item.height, out h);
				int.TryParse(item.framecount, out fc);

				float.TryParse(item.fps.Split('/')[0], out num);
				float.TryParse(item.fps.Split('/')[1], out den);

				Video.Add(new StreamVideo
				{
					Id = item.id,
					Language = string.IsNullOrEmpty(item.lang) ? "und" : item.lang,
					Codec = item.codec,
					PixelFormat = item.pixfmt,
					BitPerColour = bpc,
					Width = w,
					Height = h,
					FrameRate = num / den,
					FrameCount = fc,
				});
			}

			foreach (var item in audio)
			{
				int sample = 44100;
				int bitdepth = 16;
				int channel = 2;

				int.TryParse(item.sample, out sample);

				Regex rbit = new Regex(@"(flt|fltp|\d+)");
				Match mbit = rbit.Match(item.bitdepth);
				if (mbit.Success)
				{
					var val = mbit.Groups[1].Value;

					if (string.Equals("fltp", val, IC))
						bitdepth = 16;
					else
						int.TryParse(val, out bitdepth);

					if (bitdepth >= 32)
						bitdepth = 24;
				}

				int.TryParse(item.channel, out channel);

				Audio.Add(new StreamAudio
				{
					Id = item.id,
					Language = string.IsNullOrEmpty(item.lang) ? "und" : item.lang,
					Codec = item.codec,
					SampleRate = sample,
					BitDepth = bitdepth,
					Channel = channel,
				});
			}

			foreach (var item in subtitle)
			{
				Subtitle.Add(new StreamSubtitle
				{
					Id = item.id,
					Language = string.IsNullOrEmpty(item.lang) ? "und" : item.lang,
					Codec = item.codec,
				});
			}

			foreach (var item in attachment)
			{
				Attachment.Add(new StreamAttachment
				{
					Id = item.id,
					FileName = item.filename,
					MimeType = item.mimetype
				});
			}

			// remove xml
			File.Delete(file);
		}

		public string FormatName { get; internal set; }
		public string FormatNameFull { get; internal set; }
		public ulong FileSize { get; internal set; }
		public ulong BitRate { get; internal set; }
		public float Duration { get; internal set; }

		public List<StreamVideo> Video { get; internal set; } = new List<StreamVideo>();
		public List<StreamAudio> Audio { get; internal set; } = new List<StreamAudio>();
		public List<StreamSubtitle> Subtitle { get; internal set; } = new List<StreamSubtitle>();
		public List<StreamAttachment> Attachment { get; internal set; } = new List<StreamAttachment>();
	}
}
