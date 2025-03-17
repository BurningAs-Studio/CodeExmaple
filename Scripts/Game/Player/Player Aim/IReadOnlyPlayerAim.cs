using UnityEngine;

namespace FAS.Players
{
	public interface IReadOnlyPlayerAim
	{
		public DamageableCollider CurrentTarget {get;}
        
		public Vector3 LastAimedPosition {get;}
		
		public bool IsHasTargetInAim {get;}
		public bool IsActive {get;}
	}
}