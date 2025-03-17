using UnityEngine.UI;
using UnityEngine;
using System;
using FAS.UI;
using TMPro;

namespace FAS.LootBoxes
{
	public class LootDropScreen : UIInteractableScreen
	{
		[SerializeField] private TextMeshProUGUI _itemNameText;
		[SerializeField] private TextMeshProUGUI _itemDescriptionText;
		[SerializeField] private Image _itemImage;
		[SerializeField] private CustomButton _secondChanceButton;
		[SerializeField] private CustomButton _claimButton;

		public event Action OnSecondChanceButtonClicked;
		public event Action OnClaimButtonClicked;

		private void OnEnable()
		{
			_secondChanceButton.OnClick.AddListener(InvokeOnSecondChanceButtonClicked);
			_claimButton.OnClick.AddListener(InvokeOnClaimButtonClicked);
		}
		
		private void OnDisable()
		{
			_secondChanceButton.OnClick.RemoveListener(InvokeOnSecondChanceButtonClicked);
			_claimButton.OnClick.RemoveListener(InvokeOnClaimButtonClicked);
		}

		public void Initialize(string itemName, string itemDescription, Sprite itemIcon)
		{
			_itemNameText.text = itemName;
			_itemDescriptionText.text = itemDescription;
			_itemImage.sprite = itemIcon;
		}
		
		private void InvokeOnSecondChanceButtonClicked() => OnSecondChanceButtonClicked?.Invoke();
		
		private void InvokeOnClaimButtonClicked() => OnClaimButtonClicked?.Invoke();
	}
}