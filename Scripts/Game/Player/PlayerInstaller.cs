using System.Collections.Generic;
using FAS.Players.Animations;
using FAS.Players.AnimRig;
using FAS.Players.States;
using Cinemachine;
using FAS.MiniMap;
using UnityEngine;
using VInspector;
using Zenject;
using FAS.UI;

namespace FAS.Players
{
	[RequireComponent(typeof(PlayerVisualEffects))]
	[RequireComponent(typeof(PlayerCameraRotator))]
	[RequireComponent(typeof(CharacterController))]
	[RequireComponent(typeof(PlayerWeaponChanger))]
	[RequireComponent(typeof(PlayerSoundEffects))]
	[RequireComponent(typeof(PlayerAnimationRig))]
	[RequireComponent(typeof(PlayerCameraShaker))]
	[RequireComponent(typeof(UIScreensSwitcher))]
	[RequireComponent(typeof(PlayerSpeedBoost))]
	[RequireComponent(typeof(PlayerAnimator))]
	[RequireComponent(typeof(GroundChecker))]
	[RequireComponent(typeof(PlayerRotator))]
	[RequireComponent(typeof(PlayerPickup))]
	[RequireComponent(typeof(PlayerWeapon))]
	[RequireComponent(typeof(PlayerMover))]
	[RequireComponent(typeof(PlayerInput))]
	[RequireComponent(typeof(PlayerJump))]
	[RequireComponent(typeof(PlayerAim))]
	[RequireComponent(typeof(Health))]
	[RequireComponent(typeof(Player))]
	public class PlayerInstaller : MonoInstaller
	{
		[Tab("Common")]
		[SerializeField] private PlayerAnimEventsReceiver _animEventsReceiver;
		[SerializeField] private CharacterController _characterController;
		[SerializeField] private PunchEffectTrigger _punchEffectTrigger;
		[SerializeField] private ChangeWeaponButton _changeWeaponButton;
		[SerializeField] private UIScreensSwitcher _uiScreensSwitcher;
		[SerializeField] private PlayerCameraInputPanel _cameraInput;
		[SerializeField] private PlayerVisualEffects _visualEffects;
		[SerializeField] private PlayerWeaponChanger _weaponChanger;
		[SerializeField] private PlayerCameraRotator _cameraRotator;
		[SerializeField] private CinemachineFreeLook _followCamera;
		[SerializeField] private PlayerAnimationRig _animationRig;
		[SerializeField] private PlayerCameraShaker _cameraShaker;
		[SerializeField] private PlayerSoundEffects _soundEffects;
		[SerializeField] private PlayerAnimator _playerAnimator;
		[SerializeField] private DamageReceiver _damageReceiver;
		[SerializeField] private GroundChecker _groundChecker;
		[SerializeField] private PlayerSpeedBoost _speedBoost;
		[SerializeField] private MiniMapIcon _miniMapIcon;
		[SerializeField] private AudioSource _audioSource;
		[SerializeField] private PlayerRotator _rotator;
		[SerializeField] private PlayerPickup _pickup;
		[SerializeField] private PlayerWeapon _weapon;
		[SerializeField] private Camera _mainCamera;
		[SerializeField] private PlayerMover _mover;
		[SerializeField] private PlayerInput _input;
		[SerializeField] private PlayerJump _jump;
		[SerializeField] private PlayerAim _aim;
		[SerializeField] private Health _health;
		[SerializeField] private Player _player;
		[SerializeField] private List<Weapon> _weapons = new ();
		[Tab("StateMachine")]
		[SerializeField] private PlayerStateMachine _stateMachine;
		[SerializeField] private PlayerWeapon _playerWeapon;
		[SerializeField] private Punch _punchState;
		[SerializeField] private Respawn _respawnState;
		[SerializeField] private Attack _attackState;
		[SerializeField] private Death _deadState;
		[SerializeField] private Idle _idleState;
		[SerializeField] private Kick _kickState;
		[EndTab]
		
		public override void InstallBindings()
		{
			BindStateMachine();
			BindCommon();
		}

		private void BindStateMachine()
		{
			Container.BindInstance(_punchEffectTrigger).WhenInjectedIntoInstance(_punchState);
			Container.BindInstance(_playerWeapon).WhenInjectedIntoInstance(_attackState);
			
			Container.BindInstance(_respawnState).WhenInjectedIntoInstance(_stateMachine);
			Container.BindInstance(_attackState).WhenInjectedIntoInstance(_stateMachine);
			Container.BindInstance(_punchState).WhenInjectedIntoInstance(_stateMachine);
			Container.BindInstance(_idleState).WhenInjectedIntoInstance(_stateMachine);
			Container.BindInstance(_deadState).WhenInjectedIntoInstance(_stateMachine);
			Container.BindInstance(_kickState).WhenInjectedIntoInstance(_stateMachine);
			
			Container.BindInstance(_respawnState).WhenInjectedInto<PlayerState>();
			Container.BindInstance(_attackState).WhenInjectedInto<PlayerState>();
			Container.BindInstance(_punchState).WhenInjectedInto<PlayerState>();
			Container.BindInstance(_idleState).WhenInjectedInto<PlayerState>();
			Container.BindInstance(_deadState).WhenInjectedInto<PlayerState>();
			Container.BindInstance(_kickState).WhenInjectedInto<PlayerState>();
		}

		private void BindCommon()
		{
			BindInstanceWhenInjectedInto();
			BindFromInstance();
			BindInstance();
		}

		private void BindInstanceWhenInjectedInto()
		{
			Container.BindInstance(_followCamera).WhenInjectedIntoInstance(_cameraRotator);
			Container.BindInstance(_cameraInput).WhenInjectedIntoInstance(_cameraRotator);
			Container.BindInstance(_audioSource).WhenInjectedIntoInstance(_soundEffects);
			Container.BindInstance(_weapons).WhenInjectedIntoInstance(_weaponChanger);
			Container.BindInstance(_weaponChanger).WhenInjectedIntoInstance(_weapon);
			Container.BindInstance(_stateMachine).WhenInjectedIntoInstance(_player);
			Container.BindInstance(_cameraInput).WhenInjectedIntoInstance(_player);
			Container.BindInstance(_weapons).WhenInjectedIntoInstance(_pickup);
			Container.BindInstance(_input).WhenInjectedIntoInstance(_player);
			Container.BindInstance(_jump).WhenInjectedIntoInstance(_player);
			Container.BindInstance(_aim).WhenInjectedIntoInstance(_weapon);
			Container.BindInstance(_aim).WhenInjectedIntoInstance(_player);
			
			Container.Bind<IWeaponView>().FromInstance(_changeWeaponButton).WhenInjectedInto<Weapon>();
			
			Container.BindInstance(_cameraInput).WhenInjectedInto<CameraPanelSyncButton>();
			Container.BindInstance(_jump).WhenInjectedInto<PlayerState>();
			Container.BindInstance(_aim).WhenInjectedInto<PlayerState>();
		}
		
		private void BindFromInstance()
		{
			Container.Bind<IReadOnlyPlayerInputEvents>().FromInstance(_input.Events).AsSingle();
			Container.Bind<IReadOnlyPlayerWeapon>().FromInstance(_weapon).AsSingle();
			Container.Bind<Transform>().FromInstance(_player.transform).AsSingle();
			Container.Bind<IReadOnlyPlayerInput>().FromInstance(_input).AsSingle();
			Container.Bind<IPlayerInputControl>().FromInstance(_input).AsSingle();
			Container.Bind<IReadOnlyPlayerJump>().FromInstance(_jump).AsSingle();
			Container.Bind<IPlayerInputView>().FromInstance(_input).AsSingle();
			Container.Bind<ISpawnable>().FromInstance(_player).AsSingle();
		}
		
		private void BindInstance()
		{
			Container.BindInstance(_characterController).AsSingle();
			Container.BindInstance(_animEventsReceiver).AsSingle();
			Container.BindInstance(_uiScreensSwitcher).AsSingle();
			Container.BindInstance(_playerAnimator).AsSingle();
			Container.BindInstance(_damageReceiver).AsSingle();
			Container.BindInstance(_groundChecker).AsSingle();
			Container.BindInstance(_cameraRotator).AsSingle();
			Container.BindInstance(_visualEffects).AsSingle();
			Container.BindInstance(_animationRig).AsSingle();
			Container.BindInstance(_cameraShaker).AsSingle();
			Container.BindInstance(_soundEffects).AsSingle();
			Container.BindInstance(_miniMapIcon).AsSingle();
			Container.BindInstance(_mainCamera).AsSingle();
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
			_stateMachine = GetComponentInChildren<PlayerStateMachine>(true);
			_playerWeapon = GetComponentInChildren<PlayerWeapon>(true);
			_respawnState = GetComponentInChildren<Respawn>(true);
			_attackState = GetComponentInChildren<Attack>(true);
			_punchState = GetComponentInChildren<Punch>(true);
			_deadState = GetComponentInChildren<Death>(true);
			_idleState = GetComponentInChildren<Idle>(true);
			_kickState = GetComponentInChildren<Kick>(true);
			
			_mainCamera = GetComponentInChildren<CinemachineBrain>(true).GetComponent<Camera>();
			_animEventsReceiver = GetComponentInChildren<PlayerAnimEventsReceiver>(true);
			_characterController = GetComponentInChildren<CharacterController>(true);
			_changeWeaponButton = GetComponentInChildren<ChangeWeaponButton>(true);
			_punchEffectTrigger = GetComponentInChildren<PunchEffectTrigger>(true);
			_uiScreensSwitcher = GetComponentInChildren<UIScreensSwitcher>(true);
			_cameraInput = GetComponentInChildren<PlayerCameraInputPanel>(true);
			_weaponChanger = GetComponentInChildren<PlayerWeaponChanger>(true);
			_cameraRotator = GetComponentInChildren<PlayerCameraRotator>(true);
			_visualEffects = GetComponentInChildren<PlayerVisualEffects>(true);
			_soundEffects = GetComponentInChildren<PlayerSoundEffects>(true);
			_animationRig = GetComponentInChildren<PlayerAnimationRig>(true);
			_followCamera = GetComponentInChildren<CinemachineFreeLook>(true);
			_cameraShaker = GetComponentInChildren<PlayerCameraShaker>(true);
			_stateMachine = GetComponentInChildren<PlayerStateMachine>(true);
			_playerAnimator = GetComponentInChildren<PlayerAnimator>(true);
			_damageReceiver = GetComponentInChildren<DamageReceiver>(true);
			_groundChecker = GetComponentInChildren<GroundChecker>(true);
			_speedBoost = GetComponentInChildren<PlayerSpeedBoost>(true);
			_miniMapIcon = GetComponentInChildren<MiniMapIcon>(true);
			_audioSource = GetComponentInChildren<AudioSource>(true);
			_rotator = GetComponentInChildren<PlayerRotator>(true);
			_pickup = GetComponentInChildren<PlayerPickup>(true);
			_weapon = GetComponentInChildren<PlayerWeapon>(true);
			_mover = GetComponentInChildren<PlayerMover>(true);
			_input = GetComponentInChildren<PlayerInput>(true);
			_jump = GetComponentInChildren<PlayerJump>(true);
			_aim = GetComponentInChildren<PlayerAim>(true);
			_health = GetComponentInChildren<Health>(true);
			_player = GetComponentInChildren<Player>(true);
			
			_weapons.Clear();
			_weapons.AddRange(GetComponentsInChildren<Weapon>(true));
		}
#endif
	}
}