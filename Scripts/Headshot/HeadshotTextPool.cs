namespace FAS
{
	public class HeadshotTextPool : ObjectPool<HeadshotText>
	{
		protected override void InitializeObject(HeadshotText text)
		{
			text.Initialize();
			text.OnComplete += ReturnToPool;
		}

		protected override void CleanupObject(HeadshotText text)
		{
			text.OnComplete -= ReturnToPool;
		}
	}
}