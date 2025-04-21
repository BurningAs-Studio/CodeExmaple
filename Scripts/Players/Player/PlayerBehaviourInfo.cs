using UnityEngine;

namespace FAS.Players
{
	public class PlayerBehaviourInfo : BehaviourInfo
	{
		public override ITarget CurrentTarget { get; }
		public override bool IsHasTarget { get; }
	}
}