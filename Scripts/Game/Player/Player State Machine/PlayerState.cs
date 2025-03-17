using FAS.Players.Animations;
using FAS.Players.AnimRig;
using UnityEngine;
using Zenject;

namespace FAS.Players.States
{
	public abstract class PlayerState : State
	{
		[Inject] protected CharacterController CharacterController;
		[Inject] protected IReadOnlyPlayerInputEvents InputEvents;
		[Inject] protected PlayerAnimEventsReceiver AnimEvents;
		[Inject] protected PlayerVisualEffects VisualEffects;
		[Inject] protected PlayerCameraRotator CameraRotator;
		[Inject] protected IPlayerInputControl InputControl;
		[Inject] protected PlayerCameraShaker CameraShaker;
		[Inject] protected PlayerAnimationRig AnimationRig;
		[Inject] protected DamageReceiver DamageReceiver;
		[Inject] protected IReadOnlyPlayerWeapon Weapon;
		[Inject] protected GroundChecker GroundChecker;
		[Inject] protected IReadOnlyPlayerInput Input;
		[Inject] protected PlayerAnimator Animator;
		[Inject] protected GlobalInput GlobalInput;
		[Inject] protected PlayerRotator Rotator;
		[Inject] protected ISpawnable Spawnable;
		[Inject] protected PlayerPickup Pickup;
		[Inject] protected Transform Transform;
		[Inject] protected PlayerMover Mover;
		[Inject] protected PlayerJump Jump;
		[Inject] protected PlayerAim Aim;
		[Inject] protected Health Health;
		
		[Inject] protected Respawn RespawnState;
		[Inject] protected Attack AttackState;
		[Inject] protected Punch PunchState;
		[Inject] protected Death DeadState;
		[Inject] protected Idle IdleState;
		[Inject] protected Kick KickState;
		
		public override void Enter()
		{
			Health.OnZeroHealth += OnDead;
		}

		public override void Exit()
		{
			Health.OnZeroHealth -= OnDead;
			base.Exit();
		}


		private void OnDead()
		{
			RequestTransition(DeadState);
		}
	}
}