using System.Collections.Generic;
using FAS.Players.Animations;
using FAS.Players.AnimRig;
using FAS.Players.States;
using FAS.Fatality;
using UnityEngine;
using FAS.Weapons;
using VInspector;
using Zenject;

namespace FAS.Players
{
	[RequireComponent(typeof(FatalityTargetFinder))]
	[RequireComponent(typeof(PlayerVisualEffects))]
	[RequireComponent(typeof(CharacterController))]
	[RequireComponent(typeof(PuppetMasterHandler))]
	[RequireComponent(typeof(PlayerSoundEffects))]
	[RequireComponent(typeof(PlayerAnimationRig))]
	[RequireComponent(typeof(PlayerKickReceiver))]
	[RequireComponent(typeof(PlayerSpeedBoost))]
	[RequireComponent(typeof(FatalityTarget))]
	[RequireComponent(typeof(PlayerAnimator))]
	[RequireComponent(typeof(PlayerRotator))]
	[RequireComponent(typeof(PlayerWeapon))]
	[RequireComponent(typeof(PlayerPickup))]
	[RequireComponent(typeof(PlayerMover))]
	[RequireComponent(typeof(PlayerInput))]
	[RequireComponent(typeof(PlayerJump))]
	[RequireComponent(typeof(PlayerSkin))]
	[RequireComponent(typeof(Health))]
	[RequireComponent(typeof(Player))]
	public class PlayerInstaller : MonoInstaller
	{
		[Tab("Common")]
		[SerializeField] private PlayerUIScreensSwitcher _playerUIScreensSwitcher;
		[SerializeField] private PlayerAnimEventsReceiver _animEventsReceiver;
		[SerializeField] private FatalityTargetFinder _fatalityTargetFinder;
		[SerializeField] private PuppetMasterHandler _puppetMasterHandler;
		[SerializeField] private CharacterController _characterController;
		[SerializeField] private PunchEffectTrigger _punchEffectTrigger;
		[SerializeField] private PlayerVisualEffects _visualEffects;
		[SerializeField] private PlayerKickReceiver _kickerReceiver;
		[SerializeField] private PlayerAnimationRig _animationRig;
		[SerializeField] private PlayerSoundEffects _soundEffects;
		[SerializeField] private PlayerTargetFinder _targetFinder;
		[SerializeField] private BonesHolder _firingSkeletonBones;
		[SerializeField] private FatalityTarget _fatalityTarget;
		[SerializeField] private PlayerAnimator _playerAnimator;
		[SerializeField] private DamageReceiver _damageReceiver;
		[SerializeField] private BonesHolder _mainSkeletonBones;
		[SerializeField] private PlayerSpeedBoost _speedBoost;
		[SerializeField] private FatalityPopup _fatalityPopup;
		[SerializeField] private PlayerWeapon _playerWeapon;
		[SerializeField] private SettingsView _settingsView;
		[SerializeField] private KickTrigger _kickTrigger;
		[SerializeField] private AudioSource _audioSource;
		[SerializeField] private Animator _firingAnimator;
		[SerializeField] private Animator _mainAnimator;
		[SerializeField] private PlayerRotator _rotator;
		[SerializeField] private PlayerPickup _pickup;
		[SerializeField] private Settings _settings;
		[SerializeField] private PlayerMover _mover;
		[SerializeField] private PlayerInput _input;
		[SerializeField] private Vignette _vignette;
		[SerializeField] private PlayerSkin _skin;
		[SerializeField] private PlayerJump _jump;
		[SerializeField] private PlayerHead _head;
		[SerializeField] private Transform _body;
		[SerializeField] private Health _health;
		[SerializeField] private Player _player;
		[SerializeField] private List<Weapon> _weapons = new ();
		[Tab("StateMachine")]
		[SerializeField] private ReceiveFatality _receiveFatalityState;
		[SerializeField] private ExecuteFatality _executeFatalityState;
		[SerializeField] private PlayerStateMachine _stateMachine;
		[SerializeField] private PuppetDeath _puppetDeathState;
		[SerializeField] private Respawn _respawnState;
		[SerializeField] private Unarmed _unarmedState;
		[SerializeField] private Puppet _puppetState;
		[SerializeField] private Attack _attackState;
		[SerializeField] private Punch _punchState;
		[SerializeField] private Armed _armedState;
		[SerializeField] private Death _deadState;
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
			Container.Bind<IFatalityPopup>().FromInstance(_fatalityPopup)
				.WhenInjectedIntoInstance(_executeFatalityState);
			
			Container.Bind<IFatalityReadiness>().FromInstance(_fatalityTarget).WhenInjectedIntoInstance(_puppetState);
			Container.BindInstance(_fatalityTargetFinder).WhenInjectedIntoInstance(_executeFatalityState);
			Container.BindInstance(_fatalityTarget).WhenInjectedIntoInstance(_receiveFatalityState);
			Container.BindInstance(_punchEffectTrigger).WhenInjectedIntoInstance(_punchState);
			Container.BindInstance(_puppetDeathState).WhenInjectedIntoInstance(_puppetState);
			Container.BindInstance(_head).WhenInjectedIntoInstance(_receiveFatalityState);
			Container.BindInstance(_playerWeapon).WhenInjectedIntoInstance(_armedState);
			Container.BindInstance(_kickTrigger).WhenInjectedIntoInstance(_kickState);
			Container.BindInstance(_head).WhenInjectedIntoInstance(_respawnState);
			Container.BindInstance(_head).WhenInjectedIntoInstance(_skin);
			
			Container.BindInstance(_executeFatalityState).WhenInjectedIntoInstance(_stateMachine);
			Container.BindInstance(_receiveFatalityState).WhenInjectedIntoInstance(_stateMachine);
			Container.BindInstance(_puppetDeathState).WhenInjectedIntoInstance(_stateMachine);
			Container.BindInstance(_respawnState).WhenInjectedIntoInstance(_stateMachine);
			Container.BindInstance(_unarmedState).WhenInjectedIntoInstance(_stateMachine);
			Container.BindInstance(_puppetState).WhenInjectedIntoInstance(_stateMachine);
			Container.BindInstance(_attackState).WhenInjectedIntoInstance(_stateMachine);
			Container.BindInstance(_armedState).WhenInjectedIntoInstance(_stateMachine);
			Container.BindInstance(_punchState).WhenInjectedIntoInstance(_stateMachine);
			Container.BindInstance(_deadState).WhenInjectedIntoInstance(_stateMachine);
			Container.BindInstance(_kickState).WhenInjectedIntoInstance(_stateMachine);
			
			Container.BindInstance(_executeFatalityState).WhenInjectedInto<PlayerState>();
			Container.BindInstance(_receiveFatalityState).WhenInjectedInto<PlayerState>();
			Container.BindInstance(_unarmedState).WhenInjectedInto<PlayerState>();
			Container.BindInstance(_respawnState).WhenInjectedInto<PlayerState>();
			Container.BindInstance(_attackState).WhenInjectedInto<PlayerState>();
			Container.BindInstance(_puppetState).WhenInjectedInto<PlayerState>();
			Container.BindInstance(_punchState).WhenInjectedInto<PlayerState>();
			Container.BindInstance(_armedState).WhenInjectedInto<PlayerState>();
			Container.BindInstance(_deadState).WhenInjectedInto<PlayerState>();
			Container.BindInstance(_kickState).WhenInjectedInto<PlayerState>();
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
			Container.BindInstance(_mainAnimator)
				.WhenInjectedIntoInstance(_playerAnimator);
			
			var baseLayer = new BaseLayer(_mainAnimator, 0);
			var armLayer = new ArmLayer(_mainAnimator, 1);
			var attackLayer = new AttackLayer(_mainAnimator, 2);
			var jumpLayer = new JumpLayer(_mainAnimator, 3);
			var skillCastLayer = new SkillCastLayer(_mainAnimator, 4);
			var puppetLayer = new PuppetLayer(_mainAnimator, 5);
			var takeDamageLayer = new TakeDamageLayer(_mainAnimator, 6);
			var fatalityLayer = new FatalityLayer(_mainAnimator, 7);
			var deathLayer = new DeathLayer(_mainAnimator, 8);
			
			Container.BindInstance(takeDamageLayer).WhenInjectedIntoInstance(_playerAnimator);
			Container.BindInstance(skillCastLayer).WhenInjectedIntoInstance(_playerAnimator);
			Container.BindInstance(fatalityLayer).WhenInjectedIntoInstance(_playerAnimator);
			Container.BindInstance(attackLayer).WhenInjectedIntoInstance(_playerAnimator);
			Container.BindInstance(puppetLayer).WhenInjectedIntoInstance(_playerAnimator);
			Container.BindInstance(deathLayer).WhenInjectedIntoInstance(_playerAnimator);
			Container.BindInstance(jumpLayer).WhenInjectedIntoInstance(_playerAnimator);
			Container.BindInstance(baseLayer).WhenInjectedIntoInstance(_playerAnimator);
			Container.BindInstance(armLayer).WhenInjectedIntoInstance(_playerAnimator);
			Container.BindInstance(_weapons).WhenInjectedIntoInstance(_playerWeapon);
			Container.BindInstance(jumpLayer).WhenInjectedIntoInstance(_jump);
			
			var baseLayerFiringSkeleton = new BaseFiringAnimatorLayer(_firingAnimator, 0);
			var shootingLayerFiringSkeleton = new ArmLayer(_firingAnimator, 1);
			
			Container.BindInstance(baseLayerFiringSkeleton)
				.WhenInjectedIntoInstance(_playerAnimator);
			
			Container.BindInstance(shootingLayerFiringSkeleton)
				.WithId(AnimatorType.Firing)
				.WhenInjectedIntoInstance(_playerAnimator);
		}

		private void BindInstanceWhenInjectedInto()
		{
			Container.BindInstance(_audioSource).WhenInjectedIntoInstance(_soundEffects);
			Container.BindInstance(_settingsView).WhenInjectedIntoInstance(_settings);
			Container.BindInstance(_head).WhenInjectedIntoInstance(_kickerReceiver);
			Container.BindInstance(_stateMachine).WhenInjectedIntoInstance(_player);
			Container.BindInstance(_headsHolder).WhenInjectedIntoInstance(_pickup);
			Container.BindInstance(_input).WhenInjectedIntoInstance(_player);
			Container.BindInstance(_jump).WhenInjectedIntoInstance(_player);
			
			Container.BindInstance(_jump).WhenInjectedInto<PlayerState>();
		}
		
		private void BindFromInstance()
		{
			Container.Bind<IReadOnlyFatalityTargetFinder>().FromInstance(_fatalityTargetFinder).AsSingle();
			Container.Bind<IReadOnlyPlayerInputEvents>().FromInstance(_input.Events).AsSingle();
			Container.Bind<IReadOnlyFatalityTarget>().FromInstance(_fatalityTarget).AsSingle();
			Container.Bind<IReadOnlyPlayerWeapon>().FromInstance(_playerWeapon).AsSingle();
			Container.Bind<IReadOnlyPlayerInput>().FromInstance(_input).AsSingle();
			Container.Bind<IPlayerInputControl>().FromInstance(_input).AsSingle();
			Container.Bind<IReadOnlyPlayerJump>().FromInstance(_jump).AsSingle();
			Container.Bind<IVignettePlayer>().FromInstance(_vignette).AsSingle();
			Container.Bind<IGroundChecker>().FromInstance(_player).AsSingle();
			Container.Bind<ISpawnable>().FromInstance(_player).AsSingle();
			
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
			Container.BindInstance(_playerUIScreensSwitcher).AsSingle();
			Container.BindInstance(_puppetMasterHandler).AsSingle();
			Container.BindInstance(_characterController).AsSingle();
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
		}
		
#if UNITY_EDITOR
		[Button]
		private void FindDependencies()
		{
			_receiveFatalityState = GetComponentInChildren<ReceiveFatality>(true);
			_executeFatalityState = GetComponentInChildren<ExecuteFatality>(true);
			_stateMachine = GetComponentInChildren<PlayerStateMachine>(true);
			_puppetDeathState = GetComponentInChildren<PuppetDeath>(true);
			_respawnState = GetComponentInChildren<Respawn>(true);
			_unarmedState = GetComponentInChildren<Unarmed>(true);
			_attackState = GetComponentInChildren<Attack>(true);
			_puppetState = GetComponentInChildren<Puppet>(true);
			_armedState = GetComponentInChildren<Armed>(true);
			_punchState = GetComponentInChildren<Punch>(true);
			_deadState = GetComponentInChildren<Death>(true);
			_kickState = GetComponentInChildren<Kick>(true);
			
			_playerUIScreensSwitcher = GetComponentInChildren<PlayerUIScreensSwitcher>(true);
			_animEventsReceiver = GetComponentInChildren<PlayerAnimEventsReceiver>(true);
			_fatalityTargetFinder = GetComponentInChildren<FatalityTargetFinder>(true);
			_puppetMasterHandler = GetComponentInChildren<PuppetMasterHandler>(true);
			_characterController = GetComponentInChildren<CharacterController>(true);
			_punchEffectTrigger = GetComponentInChildren<PunchEffectTrigger>(true);
			_kickerReceiver = GetComponentInChildren<PlayerKickReceiver>(true);
			_visualEffects = GetComponentInChildren<PlayerVisualEffects>(true);
			_targetFinder = GetComponentInChildren<PlayerTargetFinder>(true);
			_soundEffects = GetComponentInChildren<PlayerSoundEffects>(true);
			_animationRig = GetComponentInChildren<PlayerAnimationRig>(true);
			_mainSkeletonBones = GetComponentInChildren<BonesHolder>(true);
			_playerAnimator = GetComponentInChildren<PlayerAnimator>(true);
			_damageReceiver = GetComponentInChildren<DamageReceiver>(true);
			_fatalityTarget = GetComponentInChildren<FatalityTarget>(true);
			_speedBoost = GetComponentInChildren<PlayerSpeedBoost>(true);
			_fatalityPopup = GetComponentInChildren<FatalityPopup>(true);
			_playerWeapon = GetComponentInChildren<PlayerWeapon>(true);
			_settingsView = GetComponentInChildren<SettingsView>(true);
			_kickTrigger = GetComponentInChildren<KickTrigger>(true);
			_audioSource = GetComponentInChildren<AudioSource>(true);
			_rotator = GetComponentInChildren<PlayerRotator>(true);
			_pickup = GetComponentInChildren<PlayerPickup>(true);
			_settings = GetComponentInChildren<Settings>(true);
			_mover = GetComponentInChildren<PlayerMover>(true);
			_input = GetComponentInChildren<PlayerInput>(true);
			_vignette = GetComponentInChildren<Vignette>(true);
			_jump = GetComponentInChildren<PlayerJump>(true);
			_head = GetComponentInChildren<PlayerHead>(true);
			_skin = GetComponentInChildren<PlayerSkin>(true);
			_health = GetComponentInChildren<Health>(true);
			_player = GetComponentInChildren<Player>(true);
			
			_weapons.Clear();
			_weapons.AddRange(GetComponentsInChildren<Weapon>(true));
		}
#endif
	}
}