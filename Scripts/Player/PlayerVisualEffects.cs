using UnityEngine;
using System;

namespace PetWorld.Player
{
    [Serializable]
    public class PlayerVisualEffects
    {
        [SerializeField] private ParticleSystem _takeDamageEffect;
        [SerializeField] private ParticleSystem _healEffect;
        
        [Header("Swimming")]
        [SerializeField] private ParticleSystem _waterSplashEffect;

        public void Initialize()
        {
            _waterSplashEffect.transform.SetParent(null);
        }

        public void PlayHealEffect()
        {
            _healEffect.Play();
        }

        public void PlayTakeDamageEffect()
        {
            _healEffect.Stop();
            _takeDamageEffect.Play();
        }

        public void PlayWaterSplashEffect(Vector3 effectPosition)
        {
            var spawnPosition = effectPosition;
            spawnPosition.y = _waterSplashEffect.transform.position.y;
            _waterSplashEffect.transform.position = spawnPosition;
            _waterSplashEffect.Play();
        }
    }
}