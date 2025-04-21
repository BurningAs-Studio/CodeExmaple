using System.Globalization;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine;
using Zenject;
using FAS.UI;
using TMPro;

namespace FAS.Players
{
	public class PlayerHealthView : UIScreen
	{
		[SerializeField] private TextMeshProUGUI _healthText;
		[SerializeField] private Image _healthImage;
		[SerializeField] private float _stepToMiddleHealthColor = 40;
		[SerializeField] private Color _middleHealthColor = Color.yellow;
		[SerializeField] private float _stepToLowerHealthColor = 20;
		[SerializeField] private Color _lowHealthColor = Color.red;
		[SerializeField] private float _animDuration = 0.2f;
		[SerializeField] private float _animScale = 1.2f;
		[SerializeField] private Ease _animEase;
		
		[Inject] private Health _health;
		
		private Tween _currentAnimTween;
		
		private readonly Color _defaultHealthColor = Color.white;

		private void OnEnable()
		{
			_health.OnTakeDamage += UpdateView;
			_health.OnTakeHeal += UpdateView;
		}

		private void OnDisable()
		{
			_health.OnTakeDamage -= UpdateView;
			_health.OnTakeHeal -= UpdateView;
		}

		private void UpdateView()
		{
			_healthText.text = Mathf.FloorToInt(_health.CurrentHealth).ToString(CultureInfo.InvariantCulture);

			if (_health.CurrentHealth > _stepToMiddleHealthColor)
				UpdateViewColor(_defaultHealthColor);
			else if (_health.CurrentHealth <= _stepToMiddleHealthColor &&
			         _health.CurrentHealth > _stepToLowerHealthColor)
				UpdateViewColor(_middleHealthColor);
			else
				UpdateViewColor(_lowHealthColor);

			PlayPunchAnimation();
		}

		private void UpdateViewColor(Color color)
		{
			_healthText.color = color;
			_healthImage.color = color;
		}

		private void PlayPunchAnimation()
		{
			if (_currentAnimTween != null)
			{
				_currentAnimTween.Kill();
				_healthText.transform.localScale = Vector3.one;
			}
			
			_currentAnimTween = _healthText.transform.DOScale(_animScale, _animDuration)
				.SetEase(_animEase)
				.SetLoops(1, LoopType.Yoyo);
		}
	}	
}