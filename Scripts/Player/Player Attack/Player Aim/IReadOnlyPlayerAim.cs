using UnityEngine;

namespace PetWorld.Player
{
    public interface IReadOnlyPlayerAim
    {
        public DamageableCollider CurrentTarget {get;}
        
        public Vector3 LastAimedPosition {get;}
		
        public bool IsHasTargetInAim {get;}
        public bool IsActive {get;}
    }
}