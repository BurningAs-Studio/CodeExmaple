using UnityEngine.UI;
using UnityEngine;
using System;

namespace PetWorld.Player
{
    [Serializable]
    public class PlayerWeaponShotFocusView
    {
        [SerializeField] private Image _focusImage;

        public void SetFade(float value)
        {
            var targetColor = _focusImage.color;
            targetColor.a = value;
            _focusImage.color = targetColor;
        }
    }
}