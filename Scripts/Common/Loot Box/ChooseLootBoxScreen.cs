using UnityEngine;
using System;
using FAS.UI;

namespace FAS.LootBoxes
{
	public class ChooseLootBoxScreen : UIInteractableScreen
	{
		[SerializeField] private CustomButton _continueButton;
		
		public event Action OnContinueButtonClicked;

		private void OnEnable() => _continueButton.OnClick.AddListener(InvokeOnContinueButtonClicked);
		
		private void OnDisable() => _continueButton.OnClick.RemoveListener(InvokeOnContinueButtonClicked);
		
		private void InvokeOnContinueButtonClicked() => OnContinueButtonClicked?.Invoke();
	}	
}