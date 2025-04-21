using System;
using FAS.Fatality;
using UnityEngine;
using Zenject;

namespace FAS.UI
{
	[RequireComponent(typeof(CanvasGroup))]
	public class FatalityButton : CustomButton
	{
		[Inject] private IReadOnlyFatalityTargetFinder _targetFinder;
		
		private CanvasGroup _canvasGroup;

		protected override void Awake()
		{
			base.Awake();
			_canvasGroup = GetComponent<CanvasGroup>();
			Hide();
		}

		private void OnEnable()
		{
			_targetFinder.OnFindFirstTarget += Show;
			_targetFinder.OnLoseAllTarget += Hide;
		}

		private void OnDisable()
		{
			_targetFinder.OnFindFirstTarget -= Show;
			_targetFinder.OnLoseAllTarget -= Hide;
		}

		private void Show()
		{
			_canvasGroup.alpha = 1;
			_canvasGroup.interactable = true;
			_canvasGroup.blocksRaycasts = true;
		}

		private void Hide()
		{
			_canvasGroup.alpha = 0;
			_canvasGroup.interactable = false;
			_canvasGroup.blocksRaycasts = false;
		}
	}
}