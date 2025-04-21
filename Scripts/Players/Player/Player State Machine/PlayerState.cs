using FAS.Players.Animations;
using FAS.Players.AnimRig;
using FAS.Fatality;
using UnityEngine;
using Zenject;

namespace FAS.Players.States
{
	public abstract class PlayerState : State
	{
		[Inject (Id = CharacterTransformType.Root)] protected Transform Transform;
		[Inject (Id = CharacterTransformType.Body)] protected Transform Body;
		[Inject] protected PlayerUIScreensSwitcher UIScreensSwitcher;
		[Inject] protected CharacterController CharacterController;
		[Inject] protected IReadOnlyPlayerInputEvents InputEvents;
		[Inject] protected IReadOnlyFatalityTarget FatalityTarget;
		[Inject] protected PlayerAnimEventsReceiver AnimEvents;
		[Inject] protected PlayerVisualEffects VisualEffects;
		[Inject] protected PlayerCameraRotator CameraRotator;
		[Inject] protected IPlayerInputControl InputControl;
		[Inject] protected PlayerKickReceiver KickReceiver;
		[Inject] protected CamerasSwitcher CamerasSwitcher;
		[Inject] protected PlayerCameraShaker CameraShaker;
		[Inject] protected PlayerTargetFinder TargetFinder;
		[Inject] protected PlayerAnimationRig AnimationRig;
		[Inject] protected PlayerSoundEffects SoundEffects;
		[Inject] protected DamageReceiver DamageReceiver;
		[Inject] protected IReadOnlyPlayerWeapon Weapon;
		[Inject] protected IReadOnlyPlayerInput Input;
		[Inject] protected IVignettePlayer Vignette;
		[Inject] protected PlayerAnimator Animator;
		[Inject] protected GlobalInput GlobalInput;
		[Inject] protected PlayerRotator Rotator;
		[Inject] protected ISpawnable Spawnable;
		[Inject] protected PlayerPickup Pickup;
		[Inject] protected Camera MainCamera;
		[Inject] protected PlayerMover Mover;
		[Inject] protected PlayerJump Jump;
		[Inject] protected Health Health;
		
		[Inject] protected ExecuteFatality ExecuteFatalityState;
		[Inject] protected ReceiveFatality ReceiveFatalityState;
		[Inject] protected Unarmed UnarmedState;
		[Inject] protected Respawn RespawnState;
		[Inject] protected Attack AttackState;
		[Inject] protected Puppet PuppetState;
		[Inject] protected Armed ArmedState;
		[Inject] protected Punch PunchState;
		[Inject] protected Death DeadState;
		[Inject] protected Kick KickState;
	}
}