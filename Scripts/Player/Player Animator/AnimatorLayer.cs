using UnityEngine;
using Zenject;
using System;

namespace PetWorld.Player
{
    [Serializable]
    public abstract class AnimatorLayer : ListenerSubscriber
    {
        [SerializeField] protected int LayerIndex;
        
        [Inject] protected PlayerAnimatorEventReceiver EventReceiver;
        [Inject(Id = PlayerAnimatorType.Main)] protected Animator Animator;
        
        public bool IsActive => 
            Animator.GetCurrentAnimatorStateInfo(LayerIndex).shortNameHash != PlayerAnimator.EmptyAnimHash;
    }
}