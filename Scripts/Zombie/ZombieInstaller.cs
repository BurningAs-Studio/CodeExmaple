using FAS.Zombies.States;
using UnityEngine.AI;
using UnityEngine;
using VInspector;
using Zenject;

namespace FAS.Zombies
{
	[RequireComponent(typeof(ZombieBehaviourInfo))]
	[RequireComponent(typeof(PuppetMasterHandler))]
	[RequireComponent(typeof(OutlineEventsSender))]
	[RequireComponent(typeof(ZombieAnimationRig))]
	[RequireComponent(typeof(ZombieAnimator))]
	[RequireComponent(typeof(DamageReceiver))]
	[RequireComponent(typeof(ZombieRotator))]
	[RequireComponent(typeof(NavMeshAgent))]
	[RequireComponent(typeof(KickReceiver))]
	[RequireComponent(typeof(ZombieMover))]
	[RequireComponent(typeof(Zombie))]
	[RequireComponent(typeof(Health))]
	public class ZombieInstaller : MonoInstaller
	{
		[Tab("Common")]
		[SerializeField] private ZombieAnimEventsReceiver _animEventsReceiver;
		[SerializeField] private PuppetMasterHandler _puppetMasterHandler;
		[SerializeField] private OutlineEventsSender _outlineEventsSender;
		[SerializeField] private ZombieAnimationRig _animationRig;
		[SerializeField] private ZombieTargetFinder _targetFinder;
		[SerializeField] private ZombieAnimator _zombieAnimator;
		[SerializeField] private DamageReceiver _damageReceiver;
		[SerializeField] private KickReceiver _kickerReceiver;
		[SerializeField] private NavMeshAgent _navMeshAgent;
		[SerializeField] private ZombieBehaviourInfo _info;
		[SerializeField] private BonesHolder _bonesHolder;
		[SerializeField] private ZombieRotator _rotator;
		[SerializeField] private ZombieMover _mover;
		[SerializeField] private Animator _animator;
		[SerializeField] private Zombie _zombie;
		[SerializeField] private Health _health;
		[EndTab]
		[Tab("State Machine")]
		[SerializeField] private MoveToDamageDealer _moveToDamageDealerState;
		[SerializeField] private ZombieStateMachine _stateMachine;
		[SerializeField] private DefaultDeath _defaultDeathState;
		[SerializeField] private PuppetDeath _puppetDeathState;
		[SerializeField] private StopAttack _stopAttackState;
		[SerializeField] private Respawn _respawnState;
		[SerializeField] private Attack _attackState;
		[SerializeField] private Patrol _patrolState;
		[SerializeField] private Puppet _puppetState;
		[SerializeField] private Chase _chaseState;
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
			Container.BindInstance(_defaultDeathState).WhenInjectedIntoInstance(_stateMachine);
			Container.BindInstance(_puppetDeathState).WhenInjectedIntoInstance(_stateMachine);
			Container.BindInstance(_stopAttackState).WhenInjectedIntoInstance(_stateMachine);
			Container.BindInstance(_puppetDeathState).WhenInjectedIntoInstance(_puppetState);
			Container.BindInstance(_respawnState).WhenInjectedIntoInstance(_stateMachine);
			Container.BindInstance(_patrolState).WhenInjectedIntoInstance(_stateMachine);
			Container.BindInstance(_attackState).WhenInjectedIntoInstance(_stateMachine);
			Container.BindInstance(_puppetState).WhenInjectedIntoInstance(_stateMachine);
			Container.BindInstance(_chaseState).WhenInjectedIntoInstance(_stateMachine);
			Container.BindInstance(_idleState).WhenInjectedIntoInstance(_stateMachine);
			
			Container.BindInstance(_moveToDamageDealerState).WhenInjectedInto<ZombieState>();
			Container.BindInstance(_defaultDeathState).WhenInjectedInto<ZombieState>();
			Container.BindInstance(_stopAttackState).WhenInjectedInto<ZombieState>();
			Container.BindInstance(_respawnState).WhenInjectedInto<ZombieState>();
			Container.BindInstance(_puppetState).WhenInjectedInto<ZombieState>();
			Container.BindInstance(_patrolState).WhenInjectedInto<ZombieState>();
			Container.BindInstance(_attackState).WhenInjectedInto<ZombieState>();
			Container.BindInstance(_chaseState).WhenInjectedInto<ZombieState>();
			Container.BindInstance(_idleState).WhenInjectedInto<ZombieState>();
		}

		private void BindCommons()
		{
			Container.Bind<IOutlineEvents>().FromInstance(_outlineEventsSender).WhenInjectedInto<OutlineLayerSwitcher>();
			
			Container.BindInstance(_animator).WhenInjectedIntoInstance(_zombieAnimator);
			Container.BindInstance(_navMeshAgent).WhenInjectedIntoInstance(_rotator);
			Container.BindInstance(_stateMachine).WhenInjectedIntoInstance(_zombie);
			Container.BindInstance(_navMeshAgent).WhenInjectedIntoInstance(_mover);
			Container.BindInstance(_puppetMasterHandler).AsSingle();
			Container.BindInstance(_animEventsReceiver).AsSingle();
			Container.BindInstance(_kickerReceiver).AsSingle();
			Container.BindInstance(_zombieAnimator).AsSingle();
			Container.BindInstance(_damageReceiver).AsSingle();
			Container.BindInstance(_animationRig).AsSingle();
			Container.BindInstance(_targetFinder).AsSingle();
			Container.BindInstance(_bonesHolder).AsSingle();
			Container.BindInstance(_rotator).AsSingle();
			Container.BindInstance(_health).AsSingle();
			Container.BindInstance(_mover).AsSingle();
			Container.BindInstance(_info).AsSingle();
		}

#if UNITY_EDITOR
		[Button]
		private void FindDependencies()
		{
			_animEventsReceiver = GetComponentInChildren<ZombieAnimEventsReceiver>(true);
			_moveToDamageDealerState = GetComponentInChildren<MoveToDamageDealer>(true);
			_outlineEventsSender = GetComponentInChildren<OutlineEventsSender>(true);
			_targetFinder = GetComponentInChildren<ZombieTargetFinder>(true);
			_animationRig = GetComponentInChildren<ZombieAnimationRig>(true);
			_stateMachine = GetComponentInChildren<ZombieStateMachine>(true);
			_defaultDeathState = GetComponentInChildren<DefaultDeath>(true);
			_puppetDeathState = GetComponentInChildren<PuppetDeath>(true);
			_puppetDeathState = GetComponentInChildren<PuppetDeath>(true);
			_kickerReceiver = GetComponentInChildren<KickReceiver>(true);
			_stopAttackState = GetComponentInChildren<StopAttack>(true);
			_info = GetComponentInChildren<ZombieBehaviourInfo>(true);
			_bonesHolder = GetComponentInChildren<BonesHolder>(true);
			_respawnState = GetComponentInChildren<Respawn>(true);
			_patrolState = GetComponentInChildren<Patrol>(true);
			_attackState = GetComponentInChildren<Attack>(true);
			_puppetState = GetComponentInChildren<Puppet>(true);
			_animator = GetComponentInChildren<Animator>(true);
			_chaseState = GetComponentInChildren<Chase>(true);
			_idleState = GetComponentInChildren<Idle>(true);
			
			_puppetMasterHandler = GetComponent<PuppetMasterHandler>();
			_zombieAnimator = GetComponent<ZombieAnimator>();
			_damageReceiver = GetComponent<DamageReceiver>();
			_navMeshAgent = GetComponent<NavMeshAgent>();
			_rotator = GetComponent<ZombieRotator>();
			_mover = GetComponent<ZombieMover>();
			_health = GetComponent<Health>();
			_zombie = GetComponent<Zombie>();
		}
#endif
	}
}