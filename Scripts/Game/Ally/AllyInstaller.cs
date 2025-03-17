using FAS.Players.Animations;
using UnityEngine.AI;
using UnityEngine;
using VInspector;
using Zenject;

namespace FAS.Allies
{
	[RequireComponent(typeof(NavMeshAgent))]
	[RequireComponent(typeof(AllyMover))]
	public class AllyInstaller : MonoInstaller
	{
		[SerializeField] private PlayerAnimator _playerAnimator;
		[SerializeField] private NavMeshAgent _agent;
		[SerializeField] private Animator _animator;
		[SerializeField] private AllyMover _mover;
		
		public override void InstallBindings()
		{
			Container.BindInstance(_animator).WhenInjectedIntoInstance(_playerAnimator);
			Container.BindInstance(_agent).WhenInjectedIntoInstance(_mover);
			Container.BindInstance(_playerAnimator).AsSingle();
			Container.BindInstance(_mover).AsSingle();
		}

#if UNITY_EDITOR
		[Button]
		private void FindDependencies()
		{
			_playerAnimator = GetComponentInChildren<PlayerAnimator>(true);
			_agent = GetComponentInChildren<NavMeshAgent>(true);
			_animator = GetComponentInChildren<Animator>(true);
			_mover = GetComponentInChildren<AllyMover>(true);
		}
#endif
	}	
}