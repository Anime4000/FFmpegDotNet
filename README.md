# FFmpegDotNet
Another FFmpeg compliance which reads media properties and then forward to FFmpeg to processing like encoding, extracting and more. This mainly use for *Internet Friendly Media Encoder*

## What is difference with MediaInfo?
MediaInfo is great tool to display media properties, much more details than FFmpeg it self, however I find so difficult to play with MediaInfo & FFmpeg, for example capture media properties with `MediaInfo.dll` and use that info to `FFmpeg.exe`, mostly it work just fine, until `FFmpeg` rejects because `MediaInfo` provides different what FFmpeg needs

For example media index/id, some are start from `1`, some are start from `0`. MediaInfo follow actual indexing/id but FFmpeg always start from `0`. When you try mapping, FFmpeg will reject.

## Usage
Using `FFmpegDotNet` very stright forward

### Capture media properties
```cs
FFmpeg.FFmpegProbe = Path.Combine("ffmpeg", "64", "ffprobe"); // set binary file

var info = new FFmpeg.GetInfo(@"D:\Users\Anime4000\Videos\ASDF COMP- BALLZ.mp4");

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
	Console.Write($"Pixel Format     : {item.Chroma}\n");
	Console.Write($"Bit per Colour   : {item.BitDepth}\n");
	Console.Write($"Resolution       : {item.Width}x{item.Height}\n");
	Console.Write($"Frame Rate       : {item.FrameRate:#.##}fps\n");
	Console.Write($"Frame Rate Avg   : {item.FrameRateAvg:#.##}fps\n");
	Console.Write($"Frame Rate Mode  : {(item.FrameRateConstant ? "Constant" : "Variable")}\n");
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
```

With that code, it will display like this (console)
```
Format: matroska,webm (Matroska / WebM)
File Size: 477782006

Type: Video
ID: 0
Language: jpn
Codec: h264
Pixel Format: yuv420p
Bit per Colour: 8
Resolution: 1280x720
Frame Rate: 23.97602fps

Type: Audio
ID: 1
Language: eng
Codec: aac
Sample Rate: 48000Hz
Bit Depth: 16 Bit (raw)
Channels: 2

Type: Audio
ID: 2
Language: jpn
Codec: aac
Sample Rate: 48000Hz
Bit Depth: 16 Bit (raw)
Channels: 2

Type: Subtitle
ID: 3
Language: eng
Codec: ass

Type: Subtitle
ID: 4
Language: eng
Codec: ass
```

### Processing
```cs
// Extract Attachment like fonts
new FFmpeg.Process().ExtractAttachment("/home/anime4000/kawaii.mp4", "/home/anime4000/fonts/");

// to do: doing encoding & decoding in the future
```

## Contribute
This code written in C# 7.2, so you need Visual Studio 2017

### Clone
You need clone these in same root directory, so FFmpegDotNet can link
```
git clone https://github.com/Anime4000/FFmpegDotNet
git clone https://github.com/JamesNK/Newtonsoft.Json
MSBuild
```
Then you can open Visual Studio Project file

To use in your project, is better to use "Add existing project" and dont forget to include Newtonsoft.Json as well