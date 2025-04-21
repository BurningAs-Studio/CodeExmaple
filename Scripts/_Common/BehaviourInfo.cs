using UnityEngine;

namespace FAS
{
	public abstract class BehaviourInfo : MonoBehaviour
	{
		public abstract ITarget CurrentTarget { get; }
		public abstract bool IsHasTarget { get; }
	}
}