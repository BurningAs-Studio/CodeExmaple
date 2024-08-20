using UnityEngine;
using System;

namespace PetWorld.Player
{
    [RequireComponent(typeof(Animator))]
    public class PlayerAnimatorEventReceiver : MonoBehaviour
    {
        public event Action OnReloadAnimFinished;
        public event Action OnHandStrechedOut;
        public event Action OnThrowFinished;
        
        public void AE_HandStrechedOut()
        {
            OnHandStrechedOut?.Invoke();
        }
		
        public void AE_ThrowFinished()
        {
            OnThrowFinished?.Invoke();
        }
		
        public void AE_RealodAnimFinished()
        {
            OnReloadAnimFinished?.Invoke();
        }
    }
}