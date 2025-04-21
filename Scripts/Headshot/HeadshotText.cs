using UnityEngine;
using DG.Tweening;
using System;
using TMPro;

namespace FAS
{
	public class HeadshotText : MonoBehaviour
	{
		[SerializeField] private TextMeshProUGUI _text;
		[SerializeField] private float _moveDuration = 0.5f;
		[SerializeField] private Ease _moveEase;
		[SerializeField] private Vector3 _scaleIn = Vector3.one;
		[SerializeField] private float _scaleInDuration = 0.5f;
		[SerializeField] private Ease _scaleInEase = Ease.OutBounce;
		[SerializeField] private Vector3 _scaleOut = Vector3.one;
		[SerializeField] private float _scaleOuDuration = 0.25f;
		[SerializeField] private Ease _scaleOutEase = Ease.OutBounce;
		[SerializeField] private float _fadeDuration = 0.5f;
		[SerializeField] private float _fadeDelay = 0.25f;
		[SerializeField] private Ease _fadeEase;
		
		private Vector3 _currentTargetPosition;
		public event Action<HeadshotText> OnComplete;

		public void Initialize()
		{
			gameObject.SetActive(false);
		}

		private const int MIN_ALPHA = 0;
		
		public void Play(Vector3 originPosition, Vector3 targetPosition)
		{
			_text.DOKill();
			transform.DOKill();
			
			_text.color = Color.yellow;
			transform.localScale = Vector3.zero;
			transform.position = originPosition;

			transform.DOScale(_scaleIn, _scaleInDuration)
				.SetEase(_scaleInEase)
				.OnComplete(() =>
				{
					transform.DOScale(_scaleOut, _scaleOuDuration)
						.SetEase(_scaleOutEase);
				});

			transform.DOMove(targetPosition, _moveDuration)
				.SetEase(_moveEase);
			
			_text.DOFade(MIN_ALPHA, _fadeDuration)
				.SetDelay(_fadeDelay)
				.SetEase(_fadeEase)
				.OnComplete(InvokeOnComplete);

		}

		private void InvokeOnComplete() => OnComplete?.Invoke(this);
	}
}