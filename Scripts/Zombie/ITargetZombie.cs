namespace FAS.Zombies
{
	public interface ITargetZombie : ITarget
	{
		public ZombieBehaviourInfo Info { get; }
	}
}