using UnityEngine.UI;
using UnityEngine;
using System;

namespace FAS.Players
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

		public void Hide()
		{
			foreach (var part in _crosshairParts)
				part.enabled = false;
		}
		
		public void Show()
		{
			foreach (var part in _crosshairParts)
				part.enabled = true;
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