using FAS.Players.Animations;
using FAS.Players.AnimRig;
using FAS.Fatality;
using FAS.Players;
using UnityEngine;
using Zenject;

namespace FAS.FakePlayers.States
{
	public abstract class FakePlayerState : State
	{
		[Inject (Id = CharacterTransformType.Body)] protected Transform Body;
		[Inject] protected PuppetMasterHandler PuppetMasterHandler;
		[Inject] protected FakePlayerAnimEventsReceiver AnimEvents;
		[Inject] protected IReadOnlyFatalityTarget FatalityTarget;
		[Inject] protected FakePlayerTargetFinder TargetFinder;
		[Inject] protected PlayerVisualEffects VisualEffects;
		[Inject] protected PlayerAnimationRig AnimationRig;
		[Inject] protected PlayerSoundEffects SoundEffects;
		[Inject] protected PlayerKickReceiver KickReceiver;
		[Inject] protected DamageReceiver DamageReceiver;
		[Inject] protected IGroundChecker GroundChecker;
		[Inject] protected PlayerAnimator Animator;
		[Inject] protected PlayerRotator Rotator;
		[Inject] protected FakePlayerMover Mover;
		[Inject] protected PlayerPickup Pickup;
		[Inject] protected PlayerJump Jump;
		[Inject] protected Health Health;
		
		[Inject] protected ReceiveFatality ReceiveFatalityState;
		[Inject] protected DefaultDeath DefaultDeathState;
		[Inject] protected Respawn RespawnState;
		[Inject] protected Puppet PuppetState;
		[Inject] protected Punch PunchState;
		[Inject] protected Idle IdleState;
		[Inject] protected Kick KickState;
	}
}