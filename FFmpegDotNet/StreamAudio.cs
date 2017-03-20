namespace FFmpegDotNet
{
	public class StreamAudio : StreamCommon
	{
		public int SampleRate { get; internal set; }
		public int Channel { get; internal set; }
		public int BitDepth { get; internal set; }
		public float Duration { get; internal set; }
	}
}
