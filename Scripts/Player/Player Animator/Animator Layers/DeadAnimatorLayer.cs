using UnityEngine;
using System;

namespace PetWorld.Player
{
    [Serializable]
    public class DeadAnimatorLayer : AnimatorLayer
    {
        private readonly int _deadTrigger = Animator.StringToHash("dead");
        
        public void DeadAnim()
        {
            Animator.SetTrigger(_deadTrigger);
        }
    }
}