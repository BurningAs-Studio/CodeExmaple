namespace FAS.Players.Animations
{
	public interface IReadOnlyAnimatorLayer
	{
		public float CurrentAnimNTime { get; }
		
		public bool IsTransition { get; }
		public bool IsDisabled { get; }
		public bool IsEnabled { get; }
		public bool IsActive { get; }
	}
}