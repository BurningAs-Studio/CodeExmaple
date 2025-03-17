using System.Collections;
using UnityEngine.Events;
using DG.Tweening;
using UnityEngine;
using VInspector;

namespace FAS.UI
{
	[RequireComponent(typeof(CanvasGroup))]
	public class UIScreen : MonoBehaviour
	{
		private struct AnimatedObject
		{
			public readonly RectTransform RectTransform;
			public readonly Vector3 MaxScale;

			public AnimatedObject(RectTransform rectTransform, Vector3 currentScale)
			{
				RectTransform = rectTransform;
				MaxScale = currentScale;
			}
		}

		[SerializeField] private bool _isAnimated;

		[ShowIf(nameof(_isAnimated))]
		[SerializeField] private RectTransform[] _objectsAnimatedOnShow;

		[SerializeField] private Vector3 _showAnimScaleTarget = new Vector3(0.05f, 0.05f, 0.05f);
		[SerializeField] private float _showAnimDuration = 0.5f;
		[SerializeField] private int _vibrato;
		[SerializeField] private int _elasticity;
		[SerializeField] private Ease _showAnimEaseType = Ease.OutBack;
		[SerializeField] private float _spawnObjectsDelay;

		[EndIf]

		protected CanvasGroup CanvasGroup;

		private AnimatedObject[] _animatedObjects;
		private Coroutine _currentShowRoutine;

		private bool _isEventsShowInEditor;

		public bool IsShown { get; protected set; }

		[ShowIf(nameof(_isEventsShowInEditor))]
		public UnityEvent OnStartShow;

		public UnityEvent OnStartHide;
		public UnityEvent OnHidden;
		public UnityEvent OnShown;

		[EndIf]

		protected virtual void Awake()
		{
			CanvasGroup = GetComponent<CanvasGroup>();

			_animatedObjects = new AnimatedObject[_objectsAnimatedOnShow.Length];
			for (var i = 0; i < _animatedObjects.Length; i++)
			{
				var currentObject = _objectsAnimatedOnShow[i];
				_animatedObjects[i] = new AnimatedObject(currentObject, currentObject.localScale);
			}
		}

		public virtual void Hide()
		{
			OnStartHide?.Invoke();
			CanvasGroup.alpha = 0;

			if (_isAnimated)
			{
				if (_currentShowRoutine != null)
					StopCoroutine(_currentShowRoutine);

				foreach (var animatedObject in _animatedObjects)
				{
					animatedObject.RectTransform.DOKill();
					animatedObject.RectTransform.localScale = animatedObject.MaxScale;
				}
			}

			HideComplete();
		}

		public virtual void Show()
		{
			OnStartShow?.Invoke();
			CanvasGroup.alpha = 1;

			if (_isAnimated)
				_currentShowRoutine = StartCoroutine(ShowRoutine());
			else
				ShowComplete();
		}

		private IEnumerator ShowRoutine()
		{
			foreach (var animatedObject in _animatedObjects)
			{
				ShowAnim(animatedObject);
				yield return new WaitForSeconds(_spawnObjectsDelay);
			}

			yield return new WaitForSeconds(_showAnimDuration);
			ShowComplete();
		}

		protected virtual void ShowComplete()
		{
			IsShown = true;
			OnShown?.Invoke();
		}

		protected virtual void HideComplete()
		{
			IsShown = false;
			OnHidden?.Invoke();
		}

		private void ShowAnim(AnimatedObject animatedObject)
		{
			animatedObject.RectTransform.DOPunchScale(_showAnimScaleTarget, _showAnimDuration, _vibrato, _elasticity)
				.SetEase(_showAnimEaseType);
		}

#if UNITY_EDITOR
		[Button]
		private void EventsShowInEditor()
		{
			_isEventsShowInEditor = !_isEventsShowInEditor;
		}

		[Button]
		private void TestShow()
		{
			Show();
		}

		[Button]
		private void TestHide()
		{
			Hide();
		}
#endif
	}
}