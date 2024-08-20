using UnityEngine;
using System;

namespace PetWorld.Player
{
    [Serializable]
    public class TakeDamageAnimatorLayer : AnimatorLayer
    {
        private readonly int _takeDamageAnimHash = Animator.StringToHash("Take Damage");

        public void PlayTakeDamageAnim()
        {
            Animator.Play(_takeDamageAnimHash, LayerIndex, 0f);
        }
    }
}