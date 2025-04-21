using Zenject;

namespace FAS.Zombies
{
	public class ZombieBehaviourInfo : BehaviourInfo
	{
		[Inject] private ZombieTargetFinder _targetFinder;

		public override ITarget CurrentTarget => _targetFinder.CurrentTarget;
		
		public override bool IsHasTarget => _targetFinder.IsHasTarget;
	}
}