using FAS.Players;
using UnityEngine;
using VInspector;
using Zenject;

namespace FAS
{
	public class VillageSceneInstaller : MonoInstaller
	{
		[SerializeField] private RespawnPointsHolder _respawnPointsHolder;
		[SerializeField] private HitEffectsPool _hitEffectsPool;
		[SerializeField] private PlayerHeadsPool _headsPool;
		[SerializeField] private HeadshotView _headshotView;
		[SerializeField] private Camera _mainCamera;
		
		public override void InstallBindings()
		{
			Container.Bind<IHeadshotView>().FromInstance(_headshotView).WhenInjectedInto<HeadshotHandler>();
			Container.BindInstance(_headsPool).WhenInjectedInto<PlayerHead>();
			Container.BindInstance(_respawnPointsHolder).AsSingle();
			Container.BindInstance(_hitEffectsPool).AsSingle();
			Container.BindInstance(_mainCamera).AsSingle();
		}

#if UNITY_EDITOR
		[Button]
		private void FindDependencies()
		{
			_respawnPointsHolder = FindFirstObjectByType<RespawnPointsHolder>(FindObjectsInactive.Include);
			_hitEffectsPool = FindFirstObjectByType<HitEffectsPool>(FindObjectsInactive.Include);
			_headsPool = FindFirstObjectByType<PlayerHeadsPool>(FindObjectsInactive.Include);
			_headshotView = FindFirstObjectByType<HeadshotView>(FindObjectsInactive.Include);
		}
#endif
	}
}