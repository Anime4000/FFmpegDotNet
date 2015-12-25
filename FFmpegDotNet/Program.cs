using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FFmpegDotNet
{
	class Program
	{
		static void Main(string[] args)
		{
			FFmpeg.Bin = @"D:\Users\Anime4000\Documents\GitHub\IFME\ifme\bin\Debug\plugins\ffmpeg\ffmpeg";
			FFmpeg.Probe = @"D:\Users\Anime4000\Documents\GitHub\IFME\ifme\bin\Debug\plugins\ffmpeg\ffprobe";
			var gg = new FFmpeg.Stream(@"D:\Users\Anime4000\Videos\dolby_sports_sound_demo_lossless-DWEU.mkv");

			Console.Write("FFmpegDotNet (module for IFME)\n\n");

			foreach (var item in gg.Video)
			{
				Console.Write($"{item.Id}\n{item.Language}\n{item.Codec}\n{item.PixelFormat}\n{item.BitPerColour}\n{item.Width}x{item.Height}\n{item.FrameRate}fps\n");
            }

			foreach (var item in gg.Audio)
			{
				Console.WriteLine($"{item.Id}\n{item.Language}\n{item.Codec}\n{item.SampleRate}Hz\n{item.BitDepth}\n{item.Channel}\n");
            }

			foreach (var item in gg.Subtitle)
			{
				Console.WriteLine($"{item.Id}\n{item.Language}\n{item.Codec}\n");
			}

			Console.ReadKey();
		}
	}
}
