using UnityEngine.UI;
using UnityEngine;
using System;

namespace PetWorld.Player
{
    [Serializable]
    public class PlayerCrosshairView
    {
        [SerializeField] private Image[] _crosshairParts;
        [SerializeField] private RectTransform _crosshairRectTransform;

        public void SetSize(float size)
        {
            _crosshairRectTransform.sizeDelta = new Vector2(size, size);
        }
        
        public void ChangeColorToRed()
        {
            foreach (var part in _crosshairParts)
                part.color = Color.red;
        }

        public void ChangeColorToWhite()
        {
            foreach (var part in _crosshairParts)
                part.color = Color.white;
        }

        public void SetScale(Vector3 size)
        {
            _crosshairRectTransform.localScale = size;
        }
    }
}