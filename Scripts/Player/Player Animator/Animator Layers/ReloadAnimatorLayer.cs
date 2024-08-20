using DG.Tweening;
using UnityEngine;
using System;

namespace PetWorld.Player
{
    [Serializable]
    public class ReloadAnimatorLayer : AnimatorLayer
    {
        [SerializeField] private float _enableReloadLayerDuration = 0.2f;
        [SerializeField] private float _disableReloadLayerDuration = 0.2f;
        
        private Tween _reloadLayerWeightTween;

        private readonly int _shotgunReload = Animator.StringToHash("Shotgun Reload");
        private readonly int _pistolReload = Animator.StringToHash("Pistol Reload");
        private readonly int _rifleReload = Animator.StringToHash("Rifle Reload");
        
        public override void AddListeners()
        {
            EventReceiver.OnReloadAnimFinished += DisableReloadLayer;
        }

        public override void RemoveListeners()
        {
            EventReceiver.OnReloadAnimFinished -= DisableReloadLayer;
        }
        
        public void PlayPistolReloadAnim()
        {
            Animator.Play(_pistolReload);
            EnableReloadLayer();
        }

        public void PlayRifleReloadAnim()
        {
            Animator.Play(_rifleReload);
            EnableReloadLayer();
        }

        public void PlayShotGunReloadAnim()
        {
            Animator.Play(_shotgunReload);
            EnableReloadLayer();
        }
        
        private void EnableReloadLayer()
        {
            _reloadLayerWeightTween.Kill();
            _reloadLayerWeightTween = DOVirtual.Float(Animator.GetLayerWeight(LayerIndex), 1, _enableReloadLayerDuration, 
                    weight => Animator.SetLayerWeight(LayerIndex, weight))
                .SetEase(Ease.Flash);
        }
		
        private void DisableReloadLayer()
        {
            _reloadLayerWeightTween.Kill();
            _reloadLayerWeightTween = DOVirtual.Float(Animator.GetLayerWeight(LayerIndex), 0, _disableReloadLayerDuration, 
                    weight => Animator.SetLayerWeight(LayerIndex, weight))
                .SetEase(Ease.Flash);
        }
    }
}