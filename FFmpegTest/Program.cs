using System;
using FFmpegDotNet;

namespace FFmpegTest
{
	class Program
	{
		static void Main(string[] args)
		{
			var info = new FFmpeg.Stream(@"D:\Users\Anime4000\Videos\ASDF COMP- BALLZ.mp4");

			string fmtName = info.FormatName;
			string fmtName2 = info.FormatNameFull;
			float time = info.Duration;
			ulong size = info.FileSize; // in bytes
			ulong bitRate = info.BitRate; // in bits

			Console.WriteLine($"Format: {fmtName} ({fmtName2}),\nSize: {size}bytes,\nBitrate: {bitRate}bps,\nLength: {time}sec\n");

			foreach (var item in info.Video)
			{
				Console.Write($"ID               : {item.Id}\n");
				Console.Write("Type             : Video\n");
				Console.Write($"Language         : {item.Language}\n");
				Console.Write($"Codec            : {item.Codec}\n");
				Console.Write($"Pixel Format     : {item.PixelFormat}\n");
				Console.Write($"Bit per Colour   : {item.BitPerColour}\n");
				Console.Write($"Resolution       : {item.Width}x{item.Height}\n");
				Console.Write($"Frame Rate       : {item.FrameRate:#.##}fps\n");
				Console.Write($"Frame Rate Avg   : {item.FrameRateAvg:#.##}fps\n");
				Console.Write($"Frame Rate Mode  : {(item.IsConstantFrameRate ? "Constant" : "Variable")}\n");
				Console.Write($"Frame Count      : {item.FrameCount} frame's\n");
				Console.Write($"\n");
			}

			foreach (var item in info.Audio)
			{
				Console.Write($"ID               : {item.Id}\n");
				Console.Write("Type             : Audio\n");
				Console.Write($"Language         : {item.Language}\n");
				Console.Write($"Codec            : {item.Codec}\n");
				Console.Write($"Sample Rate      : {item.SampleRate}Hz\n");
				Console.Write($"Bit Depth        : {item.BitDepth} Bit (raw)\n");
				Console.Write($"Channels         : {item.Channel}\n");
				Console.Write($"\n");
			}

			foreach (var item in info.Subtitle)
			{
				Console.Write($"ID              : {item.Id}\n");
				Console.Write("Type            : Subtitle\n");
				Console.Write($"Language        : {item.Language}\n");
				Console.Write($"Codec           : {item.Codec}\n");
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
