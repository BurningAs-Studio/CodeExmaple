namespace FAS.Fatality
{
	public interface IFatalityTarget : IReadOnlyFatalityTarget
	{
		public void StartFatality(FatalityData data);

		public void PerformFatality();
		
		public void FinishFatality();
	}
}