namespace FFmpegDotNet
{
	public class StreamVideo : StreamCommon
	{
		public int Chroma { get; internal set; }
		public int BitDepth { get; internal set; }
		public int Width { get; internal set; }
		public int Height { get; internal set; }
		public bool FrameRateConstant { get; internal set; }
		public float FrameRate { get; internal set; }
		public float FrameRateAvg { get; internal set; }
		public int FrameCount { get; internal set; }
		public float Duration { get; internal set; }
	}
}
