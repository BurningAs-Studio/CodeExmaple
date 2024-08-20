using UnityEngine;
using System;

namespace PetWorld.Player
{
    [Serializable]
    public class PlayerWeaponShotFocus
    {
        [SerializeField] private PlayerWeaponShotFocusView _view;
        [SerializeField] private float _changeFadeSpeed = 25f;
        
        public float CurrentFade {get; private set;}
        
        private const float DEFAULT_FADE = 0;
        private const float MAX_FADE = 1;
        
        public void SmoothResetFade()
        {
            CurrentFade = Mathf.MoveTowards(CurrentFade, DEFAULT_FADE, _changeFadeSpeed * Time.deltaTime);
            ChangeFade(CurrentFade);
        }
        
        public void ForceResetFade()
        {
            CurrentFade = DEFAULT_FADE;
            _view.SetFade(CurrentFade);
        }
        
        public void AddFadeStep(float step)
        {
            var targetValue = CurrentFade + step;
            
            if (targetValue > MAX_FADE)
                targetValue = MAX_FADE;
            
            ChangeFade(targetValue);
        }
        
        private void ChangeFade(float value)
        {
            CurrentFade = value;
            _view.SetFade(CurrentFade);
        }
    }
}