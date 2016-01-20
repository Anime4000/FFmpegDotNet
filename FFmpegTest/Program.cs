using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FFmpegDotNet;

namespace FFmpegTest
{
	class Program
	{
		static void Main(string[] args)
		{
			var info = new FFmpeg.Stream(@"D:\Users\Anime4000\Videos\Fast Furious 7 2015 1080p BRRip x265\Furious.7.2015.1080p.BRRip.x265.HEVC-zsewdc.mkv");

			string fmtName = info.FormatName;
			string fmtName2 = info.FormatNameFull;
			float time = info.Duration;
			ulong size = info.FileSize; // in bytes
			ulong bitRate = info.BitRate; // in bits

			foreach (var item in info.Video)
			{
				Console.Write("Type: Video\n");
				Console.Write($"ID: {item.Id}\n");
				Console.Write($"Language: {item.Language}\n");
				Console.Write($"Codec: {item.Codec}\n");
				Console.Write($"Pixel Format: {item.PixelFormat}\n");
				Console.Write($"Bit per Colour: {item.BitPerColour}\n");
				Console.Write($"Resolution: {item.Width}x{item.Height}\n");
				Console.Write($"Frame Rate: {item.FrameRate:#.##}fps\n");
				Console.Write($"Frame Count: {item.FrameCount} frame's\n");
				Console.Write($"\n");
			}

			foreach (var item in info.Audio)
			{
				Console.Write("Type: Audio\n");
				Console.Write($"ID: {item.Id}\n");
				Console.Write($"Language: {item.Language}\n");
				Console.Write($"Codec: {item.Codec}\n");
				Console.Write($"Sample Rate: {item.SampleRate}Hz\n");
				Console.Write($"Bit Depth: {item.BitDepth} Bit (raw)\n");
				Console.Write($"Channels: {item.Channel}\n");
				Console.Write($"\n");
			}

			foreach (var item in info.Subtitle)
			{
				Console.Write("Type: Subtitle\n");
				Console.Write($"ID: {item.Id}\n");
				Console.Write($"Language: {item.Language}\n");
				Console.Write($"Codec: {item.Codec}\n");
				Console.Write($"\n");
			}

			foreach (var item in info.Attachment)
			{
				Console.Write($"Attachment: {item.Id}, {item.FileName}, {item.MimeType}\n");
			}

			Console.Read();
		}
	}
}
