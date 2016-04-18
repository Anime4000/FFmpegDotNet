using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace FFmpegDotNet
{
	public class Get
	{
		StringComparison IC { get { return StringComparison.InvariantCultureIgnoreCase; } }

		public Get(string filePath)
		{
			var file = Path.Combine(Path.GetTempPath(), $"nemu_{new Random().Next(0, 999999999):D9}.xml");
			var ec = new Run().Execute($"\"{FFmpeg.Probe}\" -hide_banner -print_format xml -show_format -show_streams \"{filePath}\" > \"{file}\"", Path.GetTempPath());

            try
            {
                var xml = XDocument.Load(file);
                var format = from a in xml.Descendants("format")
                             select new
                             {
                                 fmtcode = a.Attribute("format_name").Value,
                                 fmtlong = a.Attribute("format_long_name").Value,
                                 size = a.Attribute("size").Value,
                                 bitrate = a.Attribute("bit_rate")?.Value,
                                 duration = a.Attribute("duration")?.Value,
                             };

                var video = from a in xml.Descendants("stream")
                            where string.Equals("video", (string)a.Attribute("codec_type"), IC)
                            select new
                            {
                                id = (int)a.Attribute("index"),
                                lang = a.Elements("tag").SingleOrDefault(x => string.Equals(x.Attribute("key").Value, "language", IC))?.Attribute("value")?.Value,
                                codec = a.Attribute("codec_name").Value,
                                pixfmt = a.Attribute("pix_fmt")?.Value,
                                bpc = a.Attribute("bits_per_raw_sample")?.Value,
                                width = a.Attribute("width").Value,
                                height = a.Attribute("height").Value,
                                fps = a.Attribute("r_frame_rate")?.Value,
                                afps = a.Attribute("avg_frame_rate")?.Value,
                                framecount = a.Attribute("nb_frames")?.Value,
                            };

                var audio = from a in xml.Descendants("stream")
                            where string.Equals("audio", (string)a.Attribute("codec_type"), IC)
                            select new
                            {
                                id = (int)a.Attribute("index"),
                                lang = a.Elements("tag").SingleOrDefault(x => string.Equals(x.Attribute("key").Value, "language", IC))?.Attribute("value")?.Value,
                                codec = a.Attribute("codec_name").Value,
                                sample = a.Attribute("sample_rate").Value,
                                bitdepth = a.Attribute("sample_fmt").Value,
                                channel = a.Attribute("channels").Value,
                            };

                var subtitle = from a in xml.Descendants("stream")
                               where string.Equals("subtitle", (string)a.Attribute("codec_type"), IC)
                               select new
                               {
                                   id = (int)a.Attribute("index"),
                                   lang = a.Elements("tag").SingleOrDefault(x => string.Equals(x.Attribute("key").Value, "language", IC))?.Attribute("value")?.Value,
                                   codec = a.Attribute("codec_name").Value,
                               };

                var attachment = from a in xml.Descendants("stream")
                                 where string.Equals("attachment", (string)a.Attribute("codec_type"), IC)
                                 select new
                                 {
                                     id = (int)a.Attribute("index"),
                                     filename = a.Elements("tag").SingleOrDefault(x => string.Equals(x.Attribute("key").Value, "filename", IC))?.Attribute("value")?.Value,
                                     mimetype = a.Elements("tag").SingleOrDefault(x => string.Equals(x.Attribute("key").Value, "mimetype", IC))?.Attribute("value")?.Value,

                                 };

                FilePath = file;

                foreach (var item in format)
                {
                    ulong filesize = 0;
                    ulong bitrate = 0;
                    float time = 0;

                    ulong.TryParse(item.size, out filesize);
                    ulong.TryParse(item.bitrate, out bitrate);
                    float.TryParse(item.duration, out time);

                    FormatName = item.fmtcode;
                    FormatNameFull = item.fmtlong;
                    FileSize = filesize;
                    BitRate = bitrate;
                    Duration = time;

                    break; // single only
                }

                foreach (var item in video)
                {
                    int bpc = 8;
                    int pix = 420;
                    int w = 0;
                    int h = 0;
                    int fc = 0;

                    int.TryParse(item.bpc, out bpc);
                    int.TryParse(item.width, out w);
                    int.TryParse(item.height, out h);
                    int.TryParse(item.framecount, out fc);

                    float num = 0;
                    float den = 0;

                    float.TryParse(item.fps.Split('/')[0], out num);
                    float.TryParse(item.fps.Split('/')[1], out den);

                    float fps = num / den;

                    float.TryParse(item.afps.Split('/')[0], out num);
                    float.TryParse(item.afps.Split('/')[1], out den);

                    float afps = num / den;

                    if (string.IsNullOrEmpty(item.pixfmt))
                    {
                        var mpix = Regex.Match(item.pixfmt, @"yuv(\d+)");
                        if (mpix.Success)
                            int.TryParse(mpix.Groups[1].Value, out pix);
                        else
                            pix = 420;
                    }

                    if (bpc == 0)
                    {
                        var mbpc = Regex.Match(item.pixfmt, @"yuv\d+p(\d+)");
                        if (mbpc.Success)
                            int.TryParse(mbpc.Groups[1].Value, out bpc);
                        else
                            bpc = 8;
                    }

                    Video.Add(new StreamVideo
                    {
                        Id = item.id,
                        Language = string.IsNullOrEmpty(item.lang) ? "und" : item.lang,
                        Codec = item.codec,
                        Chroma = pix,
                        BitDepth = bpc,
                        Width = w,
                        Height = h,
                        IsConstantFrameRate = fps == afps,
                        FrameRate = fps,
                        FrameRateAvg = afps,
                        FrameCount = fc,
                    });
                }

                foreach (var item in audio)
                {
                    int sample = 44100;
                    int bitdepth = 16;
                    int channel = 2;

                    int.TryParse(item.sample, out sample);
                    int.TryParse(item.bitdepth, out bitdepth);
                    int.TryParse(item.channel, out channel);

                    if (bitdepth == 0)
                        bitdepth = 16;

                    if (bitdepth >= 32)
                        bitdepth = 24;

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

                // error display
                if (ec > 0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write(Run.errorString);
                    Console.ResetColor();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public string FilePath { get; internal set; }
		public ulong FileSize { get; internal set; }
		public ulong BitRate { get; internal set; }
		public float Duration { get; internal set; }
        public string FormatName { get; internal set; }
        public string FormatNameFull { get; internal set; }

        public List<StreamVideo> Video { get; internal set; } = new List<StreamVideo>();
		public List<StreamAudio> Audio { get; internal set; } = new List<StreamAudio>();
		public List<StreamSubtitle> Subtitle { get; internal set; } = new List<StreamSubtitle>();
		public List<StreamAttachment> Attachment { get; internal set; } = new List<StreamAttachment>();
	}
}
