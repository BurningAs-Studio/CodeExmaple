using FAS.Players.Animations;
using UnityEngine;
using Zenject;

namespace FAS.Players
{
	public class Player : MonoBehaviour, ISpawnable, ITarget
	{
		[Inject] private CharacterController _characterController;
		[Inject] private PlayerAnimEventsReceiver _animatorEvents;
		[Inject] private PlayerVisualEffects _visualEffects;
		[Inject] private UIScreensSwitcher _uiScreensSwitcher;
		[Inject] private PlayerCameraInputPanel _cameraInput;
		[Inject] private PlayerSoundEffects _soundEffects;
		[Inject] private PlayerCameraRotator _cameraRotator;
		[Inject] private PlayerStateMachine _stateMachine;
		[Inject] private PlayerCameraShaker _cameraShaker;
		[Inject] private DamageReceiver _damageReceiver;
		[Inject] private GroundChecker _groundChecker;
		[Inject] private PlayerAnimator _animator;
		[Inject] private PlayerRotator _rotator;
		[Inject] private PlayerMover _mover;
		[Inject] private PlayerInput _input;
		[Inject] private PlayerJump _jump;
		[Inject] private PlayerAim _aim;
		[Inject] private Health _health;

		public Vector3 SpawnPosition { get; private set; }
		public Vector3 Position => transform.position;


		private void OnEnable()
		{
			_animatorEvents.OnFootstep += _soundEffects.PlayFootstepsSound;
			_health.OnTakeDamage += OnTakeDamage;
		}

		private void OnDisable()
		{
			_animatorEvents.OnFootstep -= _soundEffects.PlayFootstepsSound;
			_health.OnTakeDamage -= OnTakeDamage;
		}

		private void Start()
		{
			SpawnPosition = transform.position;
			_stateMachine.Initialize();
		}

		public void TakeDamage(float damage, DamageReceiver damageDealer)
			=> _damageReceiver.TryTakeDamage(damage, damageDealer);

		private void OnTakeDamage()
		{
			_visualEffects.PlayTakeDamageEffect(_damageReceiver.LastDamage);
			_cameraShaker.PlayTakeDamageImpulse();
			_soundEffects.PlayTakeDamageSound();
			_animator.PlayTakeDamageAnim();
		}

		private void LateUpdate()
		{
			if (_cameraInput.IsInputProcess)
				_cameraRotator.ManualRotation();
		}
	}
}