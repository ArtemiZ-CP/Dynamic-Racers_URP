public class FinishChunk : Chunk
{

	public override int SetChunkLength(int length)
	{
		length = base.SetChunkLength(length);

		return length;
	}

	public override int SetChunkHeight(int height)
	{
		return 0;
	}

	public override int SetChunkWidth(int width)
	{
		width = base.SetChunkWidth(width);

		return width;
	}
}
