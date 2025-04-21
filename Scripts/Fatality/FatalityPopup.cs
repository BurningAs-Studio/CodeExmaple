using UnityEngine.UI;
using DG.Tweening;
using UnityEngine;
using FAS.UI;

namespace FAS.Fatality
{
	public class FatalityPopup : UIScreen, IFatalityPopup
	{
		[SerializeField] private Image _fatalityImage;
		[SerializeField] private float _showDuration = 1f;
		[SerializeField] private Ease _showEase = Ease.Linear;
		[SerializeField] private float _hideDelay = 3f;
		
		private Tween _currentTween;
		
		private float _nextHideTime;

		private bool _isPopupShown;

		protected override void Awake()
		{
			base.Awake();
			Hide();
		}

		public override void Show()
		{
			base.Show();
			_currentTween = DOVirtual.Float(0, 1, _showDuration, 
					value => _fatalityImage.fillAmount = value)
				.SetEase(_showEase)
				.OnComplete(() =>
				{
					_nextHideTime = Time.timeSinceLevelLoad + _hideDelay;
					_isPopupShown = true;
				});
		}

		protected override void HideComplete()
		{
			base.HideComplete();
			_currentTween?.Kill();
			_fatalityImage.fillAmount = 0;
			_isPopupShown = false;
		}

		private void Update()
		{
			if (_isPopupShown && Time.timeSinceLevelLoad > _nextHideTime)
			{
				Hide();
				_isPopupShown = false;
			}
		}
	}
}