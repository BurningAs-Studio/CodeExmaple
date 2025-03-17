using Zenject;

namespace FAS.Zombies.States
{
	public abstract class ZombieState : State
	{
		[Inject] protected ZombieAnimEventsReceiver AnimEventsReceiver;
		[Inject] protected ZombieTargetFinder TargetFinder;
		[Inject] protected ZombieAnimator Animator;
		[Inject] protected ZombieRotator Rotator;
		[Inject] protected ZombieMover Mover;
		[Inject] protected Health Health;
		
		[Inject] protected MoveToDamageDealer MoveToDamageDealerState;
		[Inject] protected DamageReceiver DamageReceiver;
		[Inject] protected StopAttack StopAttackState;
		[Inject] protected Respawn RespawnState;
		[Inject] protected Attack AttackState;
		[Inject] protected Patrol PatrolState;
		[Inject] protected Chase ChaseState;
		[Inject] protected Death DeathState;
		[Inject] protected Idle IdleState;

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
			RequestTransition(DeathState);
		}
	}
}