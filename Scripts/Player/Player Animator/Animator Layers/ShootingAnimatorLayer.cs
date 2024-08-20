using UnityEngine;
using Zenject;
using System;

namespace PetWorld.Player
{
    [Serializable]
    public class ShootingAnimatorLayer : AnimatorLayer
    {
        [Inject(Id = PlayerAnimatorType.Shooting)] private Animator _shootingAnimator;
        
        private readonly int _assaultRifleShootAnimHash = Animator.StringToHash("Assault Rifle Shoot");
        private readonly int _pistolShootAnimHash = Animator.StringToHash("Pistol Shoot");
        
        public int ShootingLayerAnimHash { get; private set; }
        
        public void PlayPistolShootAnim()
        {
            Animator.Play(_pistolShootAnimHash, LayerIndex, 0);
            _shootingAnimator.Play(_pistolShootAnimHash, LayerIndex, 0);
        }

        public void PlayRifleShootAnim()
        {
            Animator.Play(_assaultRifleShootAnimHash, LayerIndex, 0);
            _shootingAnimator.Play(_assaultRifleShootAnimHash, LayerIndex, 0);
        }
        
        public void ResetFiringAnim()
        {
            _shootingAnimator.Play(PlayerAnimator.EmptyAnimHash, LayerIndex, 0);
        }

        public void UpdateStateInfo()
        {
            ShootingLayerAnimHash = Animator.GetCurrentAnimatorStateInfo(LayerIndex).shortNameHash;
        }
    }
}