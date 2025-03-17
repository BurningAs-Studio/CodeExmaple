using FAS.Zombies.States;
using UnityEngine.AI;
using UnityEngine;
using VInspector;
using Zenject;

namespace FAS.Zombies
{
	[RequireComponent(typeof(ZombieAnimator))]
	[RequireComponent(typeof(ZombieRotator))]
	[RequireComponent(typeof(NavMeshAgent))]
	[RequireComponent(typeof(ZombieMover))]
	[RequireComponent(typeof(Animator))]
	[RequireComponent(typeof(Zombie))]
	[RequireComponent(typeof(Health))]
	public class ZombieInstaller : MonoInstaller
	{
		[Tab("Common")]
		[SerializeField] private ZombieAnimEventsReceiver _animEventsReceiver;
		[SerializeField] private ZombieTargetFinder _targetFinder;
		[SerializeField] private ZombieAnimator _zombieAnimator;
		[SerializeField] private DamageReceiver _damageReceiver;
		[SerializeField] private NavMeshAgent _navMeshAgent;
		[SerializeField] private ZombieRotator _rotator;
		[SerializeField] private ZombieMover _mover;
		[SerializeField] private Animator _animator;
		[SerializeField] private Zombie _zombie;
		[SerializeField] private Health _health;
		[EndTab]
		[Tab("State Machine")]
		[SerializeField] private MoveToDamageDealer _moveToDamageDealerState;
		[SerializeField] private Patrol _patrolState;
		[SerializeField] private ZombieStateMachine _stateMachine;
		[SerializeField] private StopAttack _stopAttackState;
		[SerializeField] private Respawn _respawnState;
		[SerializeField] private Attack _attackState;
		[SerializeField] private Chase _chaseState;
		[SerializeField] private Death _deathState;
		[SerializeField] private Idle _idleState;
		[EndTab]
		
		public override void InstallBindings()
		{
			BindStateMachine();
			BindCommons();
		}

		private void BindStateMachine()
		{
			Container.BindInstance(_moveToDamageDealerState).WhenInjectedIntoInstance(_stateMachine);
			Container.BindInstance(_patrolState).WhenInjectedIntoInstance(_stateMachine);
			Container.BindInstance(_stopAttackState).WhenInjectedIntoInstance(_stateMachine);
			Container.BindInstance(_respawnState).WhenInjectedIntoInstance(_stateMachine);
			Container.BindInstance(_attackState).WhenInjectedIntoInstance(_stateMachine);
			Container.BindInstance(_deathState).WhenInjectedIntoInstance(_stateMachine);
			Container.BindInstance(_chaseState).WhenInjectedIntoInstance(_stateMachine);
			Container.BindInstance(_idleState).WhenInjectedIntoInstance(_stateMachine);
			
			Container.BindInstance(_moveToDamageDealerState).WhenInjectedInto<ZombieState>();
			Container.BindInstance(_patrolState).WhenInjectedInto<ZombieState>();
			Container.BindInstance(_stopAttackState).WhenInjectedInto<ZombieState>();
			Container.BindInstance(_respawnState).WhenInjectedInto<ZombieState>();
			Container.BindInstance(_attackState).WhenInjectedInto<ZombieState>();
			Container.BindInstance(_deathState).WhenInjectedInto<ZombieState>();
			Container.BindInstance(_chaseState).WhenInjectedInto<ZombieState>();
			Container.BindInstance(_idleState).WhenInjectedInto<ZombieState>();
		}

		private void BindCommons()
		{
			Container.BindInstance(_animator).WhenInjectedIntoInstance(_zombieAnimator);
			Container.BindInstance(_navMeshAgent).WhenInjectedIntoInstance(_rotator);
			Container.BindInstance(_stateMachine).WhenInjectedIntoInstance(_zombie);
			Container.BindInstance(_navMeshAgent).WhenInjectedIntoInstance(_mover);
			Container.BindInstance(_animEventsReceiver).AsSingle();
			Container.BindInstance(_zombieAnimator).AsSingle();
			Container.BindInstance(_damageReceiver).AsSingle();
			Container.BindInstance(_targetFinder).AsSingle();
			Container.BindInstance(_rotator).AsSingle();
			Container.BindInstance(_health).AsSingle();
			Container.BindInstance(_mover).AsSingle();
		}

#if UNITY_EDITOR
		[Button]
		private void FindDependencies()
		{
			_moveToDamageDealerState = GetComponentInChildren<MoveToDamageDealer>(true);
			_patrolState = GetComponentInChildren<Patrol>(true);
			_stateMachine = GetComponentInChildren<ZombieStateMachine>(true);
			_targetFinder = GetComponentInChildren<ZombieTargetFinder>(true);
			_stopAttackState = GetComponentInChildren<StopAttack>(true);
			_respawnState = GetComponentInChildren<Respawn>(true);
			_attackState = GetComponentInChildren<Attack>(true);
			_chaseState = GetComponentInChildren<Chase>(true);
			_deathState = GetComponentInChildren<Death>(true);
			_idleState = GetComponentInChildren<Idle>(true);
			
			_animEventsReceiver = GetComponent<ZombieAnimEventsReceiver>();
			_zombieAnimator = GetComponent<ZombieAnimator>();
			_damageReceiver = GetComponent<DamageReceiver>();
			_navMeshAgent = GetComponent<NavMeshAgent>();
			_rotator = GetComponent<ZombieRotator>();
			_mover = GetComponent<ZombieMover>();
			_animator = GetComponent<Animator>();
			_health = GetComponent<Health>();
			_zombie = GetComponent<Zombie>();
		}
#endif
	}
}