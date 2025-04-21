using RootMotion.Dynamics;
using FAS.Fatality;
using UnityEngine;
using Zenject;

namespace FAS.Players.States
{
	public class Puppet : PlayerState
	{
		[SerializeField] private float _maxTimeToForceStandUp = 10f;
		
		[Inject] protected PuppetMasterHandler PuppetMasterHandler;
		[Inject] private IFatalityReadiness _fatalityReadiness;
		[Inject] private PuppetDeath _puppetDeathState;

		private bool _isRegainedBalance;
		
		private float _timeToForceStandUp;
		
		public override void Enter()
		{
			FatalityTarget.OnReceivedFatality += OnReceivedFatality;
			PuppetMasterHandler.OnRegainBalance += OnRegainBalance;
			PuppetMasterHandler.OnTryGetUp += OnTryGetUp;
			Health.OnTakeDamage += OnTakeDamage;
			Health.OnZeroHealth += OnDead;
			
			Animator.SetLocomotionValue(0);
			PuppetMasterHandler.SetPinWeight(0);
			PuppetMasterHandler.SetMode(PuppetMaster.Mode.Active);
			PuppetMasterHandler.PuppetTransform.SetParent(null);
			Body.SetParent(null);
			
			_timeToForceStandUp = Time.timeSinceLevelLoad + _maxTimeToForceStandUp;

			if (Health.CurrentHealth <= 0)
				OnDead();
			else
				_fatalityReadiness.SetReadyToFatality();
		}
		
		private void OnReceivedFatality() => RequestTransition(ReceiveFatalityState);
		
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
			if (Time.timeSinceLevelLoad > _timeToForceStandUp)
			{
				ForceStandUp();
				RequestTransition(UnarmedState);
			}
			
			transform.parent.SetPositionAndRotation(Body.position, Body.rotation);

			if (_isRegainedBalance && !Animator.PuppetLayer.IsActive)
				RequestTransition(UnarmedState);
		}

		private void ForceStandUp()
		{
			Animator.PlayStandUpAnim();
			PuppetMasterHandler.Restore();
			transform.parent.SetPositionAndRotation(Body.position, Body.rotation);
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