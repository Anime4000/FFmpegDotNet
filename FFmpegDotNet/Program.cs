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
			var info = new FFmpeg.Stream(@"D:\Users\Anime4000\Videos\Anime\Girls und Panzer\[CBM]_Girls_und_Panzer_-_02_-_Tanks,_We_Ride_Them!_[720p]_[8C8775B0].mkv");

			Console.Write("FFmpegDotNet (module for IFME)\n\n");

			Console.WriteLine($"Format: {info.FormatName} ({info.FormatNameFull})");
			Console.WriteLine($"File Size: {info.FileSize}\n");

			foreach (var item in info.Video)
			{
				Console.Write("Type: Video\n");
				Console.Write($"ID: {item.Id}\n");
				Console.Write($"Language: {item.Language}\n");
				Console.Write($"Codec: {item.Codec}\n");
				Console.Write($"Pixel Format: {item.PixelFormat}\n");
				Console.Write($"Bit per Colour: {item.BitPerColour}\n");
				Console.Write($"Resolution: {item.Width}x{item.Height}\n");
				Console.Write($"Frame Rate: {item.FrameRate}fps\n");
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

			Console.ReadKey();
		}
	}
}
