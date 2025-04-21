namespace FAS
{
	public enum SpeedMultiplierType
	{
		Movement,
		Animations
	}
	
	public interface ISpeedMultiplier
	{
		public bool IsMultipliedThisFrame { get; }
		
		public void SetSpeedMultiplier(float multiplier);
	}
}