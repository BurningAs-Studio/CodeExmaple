using FAS.Players.Animations;
using UnityEngine;
using Zenject;

namespace FAS.Players
{
	public class Player : MonoBehaviour, ISpawnable, ITarget, IGroundChecker
	{
		[Inject(Id = BonesSkeletonType.Main)] private BonesHolder _mainSkeletonBones;
		[Inject] private CharacterController _characterController;
		[Inject] private PlayerAnimEventsReceiver _animatorEvents;
		[Inject] private PlayerSoundEffects _soundEffects;
		[Inject] private PlayerStateMachine _stateMachine;
		[Inject] private DamageReceiver _damageReceiver;
		[Inject] private PlayerAnimator _animator;

		public TargetType Type => TargetType.Player;
		
		public Vector3 HeadPosition => _mainSkeletonBones.Head.position;
		public Vector3 Position => transform.position;
		public Vector3 SpawnPosition { get; private set; }
		
		public bool IsGrounded => _characterController.isGrounded;

		private void OnEnable()
		{
			_animatorEvents.OnFootstep += _soundEffects.PlayFootstepsSound;
		}

		private void OnDisable()
		{
			_animatorEvents.OnFootstep -= _soundEffects.PlayFootstepsSound;
		}

		private void Start()
		{
			SpawnPosition = transform.position;
			_stateMachine.Initialize();
		}

		public void TakeDamage(float damage, DamageReceiver damageDealer)
			=> _damageReceiver.TryTakeDamage(damage, BodyPart.Other, damageDealer);
	}
}