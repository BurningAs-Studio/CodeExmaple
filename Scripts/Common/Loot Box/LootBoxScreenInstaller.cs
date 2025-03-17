using UnityEngine;
using VInspector;
using Zenject;

namespace FAS.LootBoxes
{
	public class LootBoxScreenInstaller : MonoInstaller
	{
		[SerializeField] private ChooseLootBoxScreen _chooseLootBoxScreen;
		[SerializeField] private LootBoxesScreen _lootBoxesScreen;
		[SerializeField] private LootSpinScreen _lootSpinScreen;
		[SerializeField] private LootDropScreen _lootDropScreen;
		
		public override void InstallBindings()
		{
			Container.BindInstance(_chooseLootBoxScreen).WhenInjectedIntoInstance(_lootBoxesScreen);
			Container.BindInstance(_lootSpinScreen).WhenInjectedIntoInstance(_lootBoxesScreen);
			Container.BindInstance(_lootDropScreen).WhenInjectedIntoInstance(_lootBoxesScreen);
		}

#if UNITY_EDITOR
		[Button]
		private void FindDependencies()
		{
			_chooseLootBoxScreen = GetComponentInChildren<ChooseLootBoxScreen>(true);
			_lootBoxesScreen = GetComponentInChildren<LootBoxesScreen>(true);
			_lootSpinScreen = GetComponentInChildren<LootSpinScreen>(true);
			_lootDropScreen = GetComponentInChildren<LootDropScreen>(true);
		}
#endif
	}	
}