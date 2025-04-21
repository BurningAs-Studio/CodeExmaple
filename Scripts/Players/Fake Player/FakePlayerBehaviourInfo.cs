using UnityEngine;

namespace FAS.FakePlayers
{
	public class FakePlayerBehaviourInfo : BehaviourInfo
	{
		public override ITarget CurrentTarget { get; }
		public override bool IsHasTarget { get; }
	}
}