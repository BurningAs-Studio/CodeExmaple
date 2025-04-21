using FAS.Players;

namespace FAS
{
	public interface ITargetPlayer : ITarget
	{
		public PlayerBehaviourInfo Info { get; }
	}
}