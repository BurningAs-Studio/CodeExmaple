using UnityEngine.UI;
using DG.Tweening;
using UnityEngine;
using FAS.UI;

namespace FAS
{
	public class Vignette : UIScreen, IVignettePlayer
	{
		[SerializeField] private Image _image;
		
		private Tween _currentTween;
		
		public void Play(VignetteData data)
		{
			base.Show();
			
			_currentTween.Kill();
			_image.color = data.Color;
			
			_currentTween = _image.DOFade(data.MaxAlphaNormalized, data.EnableTime)
				.SetDelay(data.EnableDelay)
				.SetEase(data.EnableEase)
				.OnComplete(() =>
				{
					_image.DOFade(0, data.DisableTime)
						.SetDelay(data.PlayTime)
						.SetEase(data.DisableEase)
						.OnComplete(Hide);
				});
		}

		protected override void HideComplete()
		{
			base.HideComplete();
			_currentTween.Kill();
		}
	}
}