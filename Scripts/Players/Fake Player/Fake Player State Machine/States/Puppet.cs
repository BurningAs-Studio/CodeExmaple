using RootMotion.Dynamics;
using FAS.Fatality;
using UnityEngine;
using Zenject;

namespace FAS.FakePlayers.States
{
	public class Puppet : FakePlayerState
	{
		[SerializeField] private float _maxTimeToForceDeath = 10f;
		
		[Inject] private IFatalityReadiness _fatalityReadiness;
		[Inject] private PuppetDeath _puppetDeathState;

		private bool _isRegainedBalance;
		
		private float _timeToForceDeath;
		
		public override void Enter()
		{
			FatalityTarget.OnReceivedFatality += OnReceivedFatality;
			PuppetMasterHandler.OnRegainBalance += OnRegainBalance;
			PuppetMasterHandler.OnTryGetUp += OnTryGetUp;
			Health.OnTakeDamage += OnTakeDamage;
			Health.OnZeroHealth += OnDead;
			
			PuppetMasterHandler.SetPinWeight(0);
			PuppetMasterHandler.SetMode(PuppetMaster.Mode.Active);
			PuppetMasterHandler.PuppetTransform.SetParent(null);
			Body.SetParent(null);
			Mover.TryStopMove();
			
			_timeToForceDeath = Time.timeSinceLevelLoad + _maxTimeToForceDeath;

			if (Health.CurrentHealth <= 0)
				OnDead();
			else
				_fatalityReadiness.SetReadyToFatality();
		}
		
		private void OnReceivedFatality() => RequestTransition(ReceiveFatalityState);
		
		private void ForceStandUp()
		{
			Animator.PlayStandUpAnim();
			PuppetMasterHandler.Restore();
			transform.parent.SetPositionAndRotation(Body.position, Body.rotation);
		}
		
		private void OnTakeDamage()
		{
			SoundEffects.PlayTakeDamageSound();
			Animator.PlayTakeDamageAnim();
		}

		private void OnTryGetUp() => PuppetMasterHandler.SetPinWeight(1);

		private void OnDead() => RequestTransition(_puppetDeathState);

		private void OnRegainBalance() => _isRegainedBalance = true;

		public override void Perform()
		{
			if (Time.timeSinceLevelLoad > _timeToForceDeath)
				RequestTransition(_puppetDeathState);
			
			Mover.RequestDisableAll();

			transform.parent.SetPositionAndRotation(Body.position, Body.rotation);

			if (_isRegainedBalance && !Animator.PuppetLayer.IsActive)
			{
				// if (TargetFinder.IsHasTarget)
				// 	RequestTransition(ChaseState);
				// else
					RequestTransition(IdleState);
			}
		}

		public override void Exit()
		{
			_isRegainedBalance = false;

			if (NextState != _puppetDeathState)
			{
				if (NextState == ReceiveFatalityState)
					ForceStandUp();
				else
					KickReceiver.SetReadyToKick();
				
				PuppetMasterHandler.SetMode(PuppetMaster.Mode.Disabled);
				Body.SetParent(transform.parent, true);
				PuppetMasterHandler.PuppetTransform.SetParent(transform.parent, true);
			}
			
			_fatalityReadiness.SetNotReadyToFatality();
			Health.OnZeroHealth -= OnDead;
			Health.OnTakeDamage -= OnTakeDamage;
			PuppetMasterHandler.OnRegainBalance -= OnRegainBalance;
			FatalityTarget.OnReceivedFatality -= OnReceivedFatality;
			base.Exit();
		}
	}
}