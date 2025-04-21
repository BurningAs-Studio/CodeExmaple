using FAS.FakePlayers.States;
using FAS.Players.Animations;
using FAS.Players.AnimRig;
using UnityEngine.AI;
using FAS.Fatality;
using FAS.Players;
using UnityEngine;
using VInspector;
using Zenject;

namespace FAS.FakePlayers
{
	[RequireComponent(typeof(FakePlayerBehaviourInfo))]
	[RequireComponent(typeof(PlayerVisualEffects))]
	[RequireComponent(typeof(PuppetMasterHandler))]
	[RequireComponent(typeof(PlayerKickReceiver))]
	[RequireComponent(typeof(PlayerSoundEffects))]
	[RequireComponent(typeof(PlayerAnimationRig))]
	[RequireComponent(typeof(PlayerSpeedBoost))]
	[RequireComponent(typeof(BasePlayerInput))]
	[RequireComponent(typeof(FakePlayerMover))]
	[RequireComponent(typeof(PlayerAnimator))]
	[RequireComponent(typeof(PlayerRotator))]
	[RequireComponent(typeof(NavMeshAgent))]
	[RequireComponent(typeof(PlayerPickup))]
	[RequireComponent(typeof(PlayerSkin))]
	[RequireComponent(typeof(PlayerJump))]
	[RequireComponent(typeof(FakePlayer))]
	[RequireComponent(typeof(Health))]
	public class FakePlayerInstaller : MonoInstaller
	{
		[Tab("Common")]
		[SerializeField] private FakePlayerAnimEventsReceiver _animEventsReceiver;
		[SerializeField] private PuppetMasterHandler _puppetMasterHandler;
		[SerializeField] private FakePlayerShootingPoint _shootingPoint;
		[SerializeField] private PunchEffectTrigger _punchEffectTrigger;
		[SerializeField] private FakePlayerTargetFinder _targetFinder;
		[SerializeField] private PlayerVisualEffects _visualEffects;
		[SerializeField] private PlayerKickReceiver _kickerReceiver;
		[SerializeField] private PlayerAnimationRig _animationRig;
		[SerializeField] private PlayerSoundEffects _soundEffects;
		[SerializeField] private BonesHolder _firingSkeletonBones;
		[SerializeField] private FatalityTarget _fatalityTarget;
		[SerializeField] private PlayerAnimator _playerAnimator;
		[SerializeField] private DamageReceiver _damageReceiver;
		[SerializeField] private BonesHolder _mainSkeletonBones;
		[SerializeField] private FakePlayerBehaviourInfo _info;
		[SerializeField] private PlayerSpeedBoost _speedBoost;
		[SerializeField] private KickTrigger _kickTrigger;
		[SerializeField] private AudioSource _audioSource;
		[SerializeField] private Animator _firingAnimator;
		[SerializeField] private FakePlayerMover _mover;
		[SerializeField] private FakePlayer _fakePlayer;
		[SerializeField] private Animator _mainAnimator;
		[SerializeField] private PlayerRotator _rotator;
		[SerializeField] private BasePlayerInput _input;
		[SerializeField] private PlayerPickup _pickup;
		[SerializeField] private NavMeshAgent _agent;
		[SerializeField] private PlayerSkin _skin;
		[SerializeField] private PlayerJump _jump;
		[SerializeField] private PlayerHead _head;
		[SerializeField] private Transform _body;
		[SerializeField] private Health _health;
		[Tab("StateMachine")]
		[SerializeField] private ReceiveFatality _receiveFatalityState;
		[SerializeField] private FakePlayerStateMachine _stateMachine;
		[SerializeField] private PuppetDeath _puppetDeathState;
		[SerializeField] private DefaultDeath _deadState;
		[SerializeField] private Respawn _respawnState;
		[SerializeField] private Puppet _puppetState;
		[SerializeField] private Punch _punchState;
		[SerializeField] private Idle _idleState;
		[SerializeField] private Kick _kickState;
		[EndTab]
		
		private readonly PlayerHeadsHolder _headsHolder = new ();
		
		public override void InstallBindings()
		{
			BindStateMachine();
			BindCommon();
		}

		private void BindStateMachine()
		{
			Container.Bind<IFatalityReadiness>().FromInstance(_fatalityTarget).WhenInjectedIntoInstance(_puppetState);
			Container.BindInstance(_fatalityTarget).WhenInjectedIntoInstance(_receiveFatalityState);
			Container.BindInstance(_punchEffectTrigger).WhenInjectedIntoInstance(_punchState);
			Container.BindInstance(_puppetDeathState).WhenInjectedIntoInstance(_puppetState);
			Container.BindInstance(_head).WhenInjectedIntoInstance(_receiveFatalityState);
			Container.BindInstance(_kickTrigger).WhenInjectedIntoInstance(_kickState);
			Container.BindInstance(_head).WhenInjectedIntoInstance(_respawnState);
			Container.BindInstance(_head).WhenInjectedIntoInstance(_skin);
			
			Container.BindInstance(_receiveFatalityState).WhenInjectedIntoInstance(_stateMachine);
			Container.BindInstance(_puppetDeathState).WhenInjectedIntoInstance(_stateMachine);
			Container.BindInstance(_respawnState).WhenInjectedIntoInstance(_stateMachine);
			Container.BindInstance(_puppetState).WhenInjectedIntoInstance(_stateMachine);
			Container.BindInstance(_punchState).WhenInjectedIntoInstance(_stateMachine);
			Container.BindInstance(_idleState).WhenInjectedIntoInstance(_stateMachine);
			Container.BindInstance(_deadState).WhenInjectedIntoInstance(_stateMachine);
			Container.BindInstance(_kickState).WhenInjectedIntoInstance(_stateMachine);
			
			Container.BindInstance(_receiveFatalityState).WhenInjectedInto<FakePlayerState>();
			Container.BindInstance(_respawnState).WhenInjectedInto<FakePlayerState>();
			Container.BindInstance(_puppetState).WhenInjectedInto<FakePlayerState>();
			Container.BindInstance(_punchState).WhenInjectedInto<FakePlayerState>();
			Container.BindInstance(_idleState).WhenInjectedInto<FakePlayerState>();
			Container.BindInstance(_deadState).WhenInjectedInto<FakePlayerState>();
			Container.BindInstance(_kickState).WhenInjectedInto<FakePlayerState>();
		}

		private void BindCommon()
		{
			CreateAndBindAnimationLayers();
			BindInstanceWhenInjectedInto();
			BindFromInstance();
			BindInstance();
		}
		
		private void CreateAndBindAnimationLayers()
		{
			Container.BindInstance(_mainAnimator).WhenInjectedIntoInstance(_playerAnimator);
			
			var baseLayer = new BaseLayer(_mainAnimator, 0);
			var jumpLayer = new JumpLayer(_mainAnimator, 1);
			var shootingLayer = new ArmLayer(_mainAnimator, 2);
			var additionalShootingLayer = new Layer(_mainAnimator, 3);
			var skillCastLayer = new SkillCastLayer(_mainAnimator, 4);
			var puppetLayer = new PuppetLayer(_mainAnimator, 5);
			var takeDamageLayer = new TakeDamageLayer(_mainAnimator, 6);
			var fatalityLayer = new FatalityLayer(_mainAnimator, 7);
			var deathLayer = new DeathLayer(_mainAnimator, 8);
			
			Container.BindInstance(additionalShootingLayer).WhenInjectedIntoInstance(_playerAnimator);
			Container.BindInstance(takeDamageLayer).WhenInjectedIntoInstance(_playerAnimator);
			Container.BindInstance(skillCastLayer).WhenInjectedIntoInstance(_playerAnimator);
			Container.BindInstance(fatalityLayer).WhenInjectedIntoInstance(_playerAnimator);
			Container.BindInstance(puppetLayer).WhenInjectedIntoInstance(_playerAnimator);
			Container.BindInstance(deathLayer).WhenInjectedIntoInstance(_playerAnimator);
			Container.BindInstance(baseLayer).WhenInjectedIntoInstance(_playerAnimator);
			Container.BindInstance(jumpLayer).WhenInjectedIntoInstance(_playerAnimator);
			Container.BindInstance(jumpLayer).WhenInjectedIntoInstance(_fakePlayer);
			Container.BindInstance(jumpLayer).WhenInjectedIntoInstance(_jump);
			Container.BindInstance(shootingLayer)
				.WithId(AnimatorType.Main)
				.WhenInjectedIntoInstance(_playerAnimator);
			
			var shootingLayerFiringSkeleton = new ArmLayer(_firingAnimator, 0);
			
			Container.BindInstance(shootingLayerFiringSkeleton)
				.WithId(AnimatorType.Firing)
				.WhenInjectedIntoInstance(_playerAnimator);
		}

		private void BindInstanceWhenInjectedInto()
		{
			Container.Bind<ITargetFakePlayer>().FromInstance(_fakePlayer).WhenInjectedIntoInstance(_targetFinder);
			Container.BindInstance(_audioSource).WhenInjectedIntoInstance(_soundEffects);
			Container.BindInstance(_stateMachine).WhenInjectedIntoInstance(_fakePlayer);
			Container.BindInstance(_head).WhenInjectedIntoInstance(_kickerReceiver);
			Container.BindInstance(_headsHolder).WhenInjectedIntoInstance(_pickup);
			Container.BindInstance(_agent).WhenInjectedIntoInstance(_fakePlayer);
			Container.BindInstance(_jump).WhenInjectedIntoInstance(_fakePlayer);
			Container.BindInstance(_agent).WhenInjectedIntoInstance(_mover);
			
			Container.BindInstance(_jump).WhenInjectedInto<FakePlayerState>();
		}
		
		private void BindFromInstance()
		{
			Container.Bind<IReadOnlyPlayerInputEvents>().FromInstance(_input.Events).AsSingle();
			Container.Bind<IReadOnlyFatalityTarget>().FromInstance(_fatalityTarget).AsSingle();
			Container.Bind<IGroundChecker>().FromInstance(_fakePlayer).AsSingle();
			Container.Bind<IReadOnlyPlayerJump>().FromInstance(_jump).AsSingle();
			
			Container.Bind<ISpeedMultiplier>()
				.WithId(SpeedMultiplierType.Movement)
				.FromInstance(_mover).AsCached();
			
			Container.Bind<ISpeedMultiplier>()
				.WithId(SpeedMultiplierType.Animations)
				.FromInstance(_playerAnimator).AsCached();
		}
		
		private void BindInstance()
		{
			Container.BindInstance(_firingSkeletonBones).WithId(BonesSkeletonType.Firing).AsCached();
			Container.BindInstance(_mainSkeletonBones).WithId(BonesSkeletonType.Main).AsCached();
			Container.BindInstance(transform).WithId(CharacterTransformType.Root).AsCached();
			Container.BindInstance(_body).WithId(CharacterTransformType.Body).AsCached();
			Container.BindInstance(_puppetMasterHandler).AsSingle();
			Container.BindInstance(_animEventsReceiver).AsSingle();
			Container.BindInstance(_kickerReceiver).AsSingle();
			Container.BindInstance(_playerAnimator).AsSingle();
			Container.BindInstance(_damageReceiver).AsSingle();
			Container.BindInstance(_visualEffects).AsSingle();
			Container.BindInstance(_animationRig).AsSingle();
			Container.BindInstance(_soundEffects).AsSingle();
			Container.BindInstance(_targetFinder).AsSingle();
			Container.BindInstance(_speedBoost).AsSingle();
			Container.BindInstance(_rotator).AsSingle();
			Container.BindInstance(_health).AsSingle();
			Container.BindInstance(_pickup).AsSingle();
			Container.BindInstance(_mover).AsSingle();
			Container.BindInstance(_info).AsSingle();
		}
		
#if UNITY_EDITOR
		[Button]
		private void FindDependencies()
		{
			_receiveFatalityState = GetComponentInChildren<ReceiveFatality>(true);
			_stateMachine = GetComponentInChildren<FakePlayerStateMachine>(true);
			_puppetDeathState = GetComponentInChildren<PuppetDeath>(true);
			_deadState = GetComponentInChildren<DefaultDeath>(true);
			_respawnState = GetComponentInChildren<Respawn>(true);
			_puppetState = GetComponentInChildren<Puppet>(true);
			_punchState = GetComponentInChildren<Punch>(true);
			_idleState = GetComponentInChildren<Idle>(true);
			_kickState = GetComponentInChildren<Kick>(true);
			
			_animEventsReceiver = GetComponentInChildren<FakePlayerAnimEventsReceiver>(true);
			_puppetMasterHandler = GetComponentInChildren<PuppetMasterHandler>(true);
			_shootingPoint = GetComponentInChildren<FakePlayerShootingPoint>(true);
			_punchEffectTrigger = GetComponentInChildren<PunchEffectTrigger>(true);
			_targetFinder = GetComponentInChildren<FakePlayerTargetFinder>(true);
			_visualEffects = GetComponentInChildren<PlayerVisualEffects>(true);
			_kickerReceiver = GetComponentInChildren<PlayerKickReceiver>(true);
			_soundEffects = GetComponentInChildren<PlayerSoundEffects>(true);
			_animationRig = GetComponentInChildren<PlayerAnimationRig>(true);
			_playerAnimator = GetComponentInChildren<PlayerAnimator>(true);
			_damageReceiver = GetComponentInChildren<DamageReceiver>(true);
			_mainSkeletonBones = GetComponentInChildren<BonesHolder>(true);
			_fatalityTarget = GetComponentInChildren<FatalityTarget>(true);
			_info = GetComponentInChildren<FakePlayerBehaviourInfo>(true);
			_speedBoost = GetComponentInChildren<PlayerSpeedBoost>(true);
			_kickTrigger = GetComponentInChildren<KickTrigger>(true);
			_audioSource = GetComponentInChildren<AudioSource>(true);
			_mover = GetComponentInChildren<FakePlayerMover>(true);
			_fakePlayer = GetComponentInChildren<FakePlayer>(true);
			_rotator = GetComponentInChildren<PlayerRotator>(true);
			_pickup = GetComponentInChildren<PlayerPickup>(true);
			_agent = GetComponentInChildren<NavMeshAgent>(true);
			_skin = GetComponentInChildren<PlayerSkin>(true);
			_jump = GetComponentInChildren<PlayerJump>(true);
			_head = GetComponentInChildren<PlayerHead>(true);
			_health = GetComponentInChildren<Health>(true);
		}
#endif
	}
}