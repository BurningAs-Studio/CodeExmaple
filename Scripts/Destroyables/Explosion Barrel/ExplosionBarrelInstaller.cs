using UnityEngine;
using VInspector;
using Zenject;

namespace FAS
{
	public class ExplosionBarrelInstaller : MonoInstaller
	{
		[SerializeField] private DamageReceiver _damageReceiver;

		public override void InstallBindings()
		{
			//Container.Bind<IOutlineEventsHolder>().FromInstance(_damageReceiver).AsSingle();
		}
		
#if UNITY_EDITOR
		[Button]
		private void FindDependencies()
		{
			_damageReceiver = GetComponentInChildren<DamageReceiver>(true);
		}
#endif
	}
}