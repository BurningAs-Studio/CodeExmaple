using Zenject;

namespace FAS.Zombies.States
{
	public abstract class ZombieState : State
	{
		[Inject] protected PuppetMasterHandler PuppetMasterHandler;
		[Inject] protected ZombieAnimEventsReceiver AnimEvents;
		[Inject] protected ZombieTargetFinder TargetFinder;
		[Inject] protected KickReceiver KickReceiver;
		[Inject] protected ZombieAnimator Animator;
		[Inject] protected ZombieRotator Rotator;
		[Inject] protected ZombieMover Mover;
		[Inject] protected Health Health;
		
		[Inject] protected MoveToDamageDealer MoveToDamageDealerState;
		[Inject] protected DefaultDeath DefaultDeathState;
		[Inject] protected DamageReceiver DamageReceiver;
		[Inject] protected StopAttack StopAttackState;
		[Inject] protected Respawn RespawnState;
		[Inject] protected Attack AttackState;
		[Inject] protected Patrol PatrolState;
		[Inject] protected Puppet PuppetState;
		[Inject] protected Chase ChaseState;
		[Inject] protected Idle IdleState;

		public override void Enter()
		{
			PuppetMasterHandler.OnLoseBalance += OnLoseBalance;
			Health.OnTakeDamage += OnTakeDamage;
			KickReceiver.OnKick += OnKick;
			Health.OnZeroHealth += OnDead;
		}

		private void OnKick() => RequestTransition(PuppetState);

		protected virtual void OnTakeDamage() => Animator.PlayTakeDamageAnim();
		
		private void OnDead() => RequestTransition(DefaultDeathState);
		
		private void OnLoseBalance() => RequestTransition(PuppetState);

		public override void Exit()
		{
			Health.OnZeroHealth -= OnDead;
			KickReceiver.OnKick -= OnKick;
			Health.OnTakeDamage -= OnTakeDamage;
			PuppetMasterHandler.OnLoseBalance -= OnLoseBalance;
			base.Exit();
		}
	}
}