using FAS.Projectiles;
using UnityEngine;
using VInspector;
using Zenject;

namespace FAS
{
	public class GameSceneInstaller : MonoInstaller
	{
		[SerializeField] private RespawnPointsHolder _respawnPointsHolder;
		[SerializeField] private ProjectilePool _projectilePool;
		
		public override void InstallBindings()
		{
			Container.BindInstance(_projectilePool).WhenInjectedInto<Weapon>();
			Container.BindInstance(_respawnPointsHolder);
		}

#if UNITY_EDITOR
		[Button]
		private void FindDependencies()
		{
			_respawnPointsHolder = FindFirstObjectByType<RespawnPointsHolder>(FindObjectsInactive.Include);
			_projectilePool = FindFirstObjectByType<ProjectilePool>(FindObjectsInactive.Include);
		}
#endif
	}
}