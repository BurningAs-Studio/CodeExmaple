namespace FAS
{
	public interface IOutlineEventsSender : IOutlineEvents
	{
		public void TryDisableOutline();

		public void TryEnableOutline();
	}
}