using System;
using FAS.UI;
using Zenject;

namespace FAS.LootBoxes
{
	public class LootBoxesScreen : UIInteractableScreen
	{
		[Inject] private ChooseLootBoxScreen _chooseLootBoxScreen;
		[Inject] private LootSpinScreen _lootSpinScreen;
		[Inject] private LootDropScreen _lootDropScreen;

		public event Action OnLootClaimed;

		private void OnEnable()
		{
			_chooseLootBoxScreen.OnContinueButtonClicked += ShowLootSpinScreen;
			_lootDropScreen.OnSecondChanceButtonClicked += ShowLootSpinScreen;
			_lootDropScreen.OnClaimButtonClicked += InvokeOnLootClaimed;
			_lootSpinScreen.OnLootSelected += ShowLootDropScreen;
		}

		private void OnDisable()
		{
			_chooseLootBoxScreen.OnContinueButtonClicked -= ShowLootSpinScreen;
			_lootDropScreen.OnSecondChanceButtonClicked -= ShowLootSpinScreen;
			_lootDropScreen.OnClaimButtonClicked -= InvokeOnLootClaimed;
			_lootSpinScreen.OnLootSelected -= ShowLootDropScreen;
		}

		public override void Show()
		{
			base.Show();
			ShowChooseLootBoxesScreen();
		}

		private void ShowChooseLootBoxesScreen()
		{
			_chooseLootBoxScreen.Show();
			_lootSpinScreen.Hide();
			_lootDropScreen.Hide();
		}

		private void ShowLootSpinScreen()
		{
			_chooseLootBoxScreen.Hide();
			_lootSpinScreen.Show();
			_lootDropScreen.Hide();
		}
		
		private void ShowLootDropScreen(LootBoxItemData itemData)
		{
			_lootDropScreen.Initialize(itemData.Name, itemData.Description, itemData.DropSprite);
			_chooseLootBoxScreen.Hide();
			_lootSpinScreen.Hide();
			_lootDropScreen.Show();
		}

		private void InvokeOnLootClaimed() => OnLootClaimed?.Invoke();
	}
}