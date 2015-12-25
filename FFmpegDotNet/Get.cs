using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace FFmpegDotNet
{
	public class Get
	{
		public List<StreamVideo> Video { get; internal set; } = new List<StreamVideo>();
		public List<StreamAudio> Audio { get; internal set; } = new List<StreamAudio>();
		public List<StreamSubtitle> Subtitle { get; internal set; } = new List<StreamSubtitle>();

		public Get(string filePath)
		{
			foreach (var item in new FFmpeg.Process().Print(filePath))
			{
				string id = string.Empty;
				string lang = string.Empty;

				Regex rID = new Regex(@"[S|s]tream #(\d+:\d+)");
				Match mID = rID.Match(item);
				if (mID.Success)
				{
					id = mID.Groups[1].Value;
				}

				Regex rLANG = new Regex(@"[S|s]tream #\d+:\d+\(([a-z]{3})\)");
				Match mLANG = rLANG.Match(item);
				if (mLANG.Success)
				{
					lang = mLANG.Groups[1].Value;
				}

				Regex rVideo = new Regex("[S|s]tream #.*[V|v]ideo:");
				Match mVideo = rVideo.Match(item);
				if (mVideo.Success)
				{
					string codec = string.Empty;
					string pixFmt = string.Empty;
					int width = 0;
					int height = 0;
					float fps = 0;

					Regex rCODEC = new Regex(@"[S|s]tream #.*[V|v]ideo: (\w+)");
					Match mCODEC = rCODEC.Match(item);
					if (mCODEC.Success)
					{
						codec = mCODEC.Groups[1].Value;
					}

					Regex rPIXFMT = new Regex(@"[S|s]tream #.*[V|v]ideo:.*\), (\w+)");
					Match mPIXFMT = rPIXFMT.Match(item);
					if (mPIXFMT.Success)
					{
						pixFmt = mPIXFMT.Groups[1].Value;
					}

					Regex rRES = new Regex(@"[S|s]tream #.*[V|v]ideo:.* (\d+x\d+)");
					Match mRES = rRES.Match(item);
					if (mRES.Success)
					{
						string res = mRES.Groups[1].Value;
						int w = int.Parse(res.Split('x')[0]);
						int h = int.Parse(res.Split('x')[1]);

						width = w;
						height = h;
					}

					Regex rFPS = new Regex(@"[S|s]tream #.*[V|v]ideo:.* (\d+|\d+.\d+) *.fps");
					Match mFPS = rFPS.Match(item);
					if (mFPS.Success)
					{
						fps = float.Parse(mFPS.Groups[1].Value);
					}

					Video.Add(new StreamVideo
					{
						Id = id,
						Language = lang,
						Codec = codec,
						PixelFormat = pixFmt,
						Width = width,
						Height = height,
						FrameRate = fps
					});

					continue;
				}

				Regex rAudio = new Regex("[S|s]tream #.*[A|a]udio:");
				Match mAudio = rAudio.Match(item);
				if (mAudio.Success)
				{
					string codec = string.Empty;
					int sampleRate = 0;
					int channel = 0;
					int bitdepth = 0;

					Regex rCODEC = new Regex(@"[S|s]tream #.*[A|a]udio: (\w+)");
					Match mCODEC = rCODEC.Match(item);
					if (mCODEC.Success)
					{
						codec = mCODEC.Groups[1].Value;
					}

					Regex rSRATE = new Regex(@"[S|s]tream #.*[A|a]udio:.* (\d+) Hz,");
					Match mSRATE = rSRATE.Match(item);
					if (mSRATE.Success)
					{
						sampleRate = int.Parse(mSRATE.Groups[1].Value);
					}

					Regex rCHAN = new Regex(@"[S|s]tream #.*[A|a]udio:.* Hz, (stereo|mono|\d+|\d+\.\d+)");
					Match mCHAN = rCHAN.Match(item);
					if (mCHAN.Success)
					{
						string chan = mCHAN.Groups[1].Value;
						if (chan == "stereo")
							channel = 2;
						else if (chan == "mono")
							channel = 1;
						else if (chan.Contains('.'))
							channel = int.Parse(chan.Split('.')[0]) + int.Parse(chan.Split('.')[1]);
						else
							int.TryParse(chan, out channel);
					}

					Regex rBITD = new Regex(@"[S|s]tream #.*[A|a]udio:.*(fltp|\d\d)");
					Match mBITD = rBITD.Match(item);
					if (mBITD.Success)
					{
						string bit = mBITD.Groups[1].Value;
						if (bit == "fltp")
							bitdepth = 16;
						else
							int.TryParse(bit, out bitdepth);

						if (bitdepth >= 32)
							bitdepth = 24;
					}

					Audio.Add(new StreamAudio
					{
						Id = id,
						Language = lang,
						Codec = codec,
						SampleRate = sampleRate,
						Channel = channel,
						BitDepth = bitdepth
					});

					continue;
				}

				Regex rSub = new Regex("[S|s]tream #.*[S|s]ubtitle:");
				Match mSub = rSub.Match(item);
				if (mSub.Success)
				{
					string codec = string.Empty;

					Regex rCODEC = new Regex(@"[S|s]tream #.*[S|s]ubtitle: (\w+)");
					Match mCODEC = rCODEC.Match(item);
					if (mCODEC.Success)
					{
						codec = mCODEC.Groups[1].Value;
					}

					Subtitle.Add(new StreamSubtitle
					{
						Id = id,
						Language = lang,
						Codec = codec
					});

					continue;
				}
			}
		}
	}
}
