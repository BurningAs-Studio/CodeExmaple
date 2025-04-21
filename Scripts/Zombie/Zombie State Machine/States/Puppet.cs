using RootMotion.Dynamics;
using UnityEngine;
using Zenject;

namespace FAS.Zombies.States
{
	public class Puppet : ZombieState
	{
		[SerializeField] private float _maxTimeToForceDeath = 10f;
		
		[Inject] private PuppetDeath _puppetDeathState;

		private bool _isRegainedBalance;
		
		private float _timeToForceDeath;
		
		private bool IsPuppetAnimPlaying => Animator.BaseLayerInfo.shortNameHash == _writheAnimHash
		                                    || Animator.BaseLayerInfo.shortNameHash == _getUpProneAnimHash
		                                    || Animator.BaseLayerInfo.shortNameHash == _getUpSupineAnimHash;

		private readonly int _getUpSupineAnimHash = UnityEngine.Animator.StringToHash("GetUpSupine");
		private readonly int _getUpProneAnimHash = UnityEngine.Animator.StringToHash("GetUpProne");
		private readonly int _writheAnimHash = UnityEngine.Animator.StringToHash("Writhe");
		
		public override void Enter()
		{
			PuppetMasterHandler.OnRegainBalance += OnRegainBalance;
			PuppetMasterHandler.OnTryGetUp += OnTryGetUp;
			Health.OnTakeDamage += OnTakeDamage;
			Health.OnZeroHealth += OnDead;
			
			PuppetMasterHandler.SetPinWeight(0);
			PuppetMasterHandler.SetMode(PuppetMaster.Mode.Active);
			PuppetMasterHandler.PuppetTransform.SetParent(null);
			Mover.Model.SetParent(null);
			Mover.TryStopMove();
			
			_timeToForceDeath = Time.timeSinceLevelLoad + _maxTimeToForceDeath;

			if (Health.CurrentHealth <= 0)
				OnDead();
		}

		private void OnTryGetUp() => PuppetMasterHandler.SetPinWeight(1);

		private void OnDead() => RequestTransition(_puppetDeathState);

		private void OnRegainBalance() => _isRegainedBalance = true;

		public override void Perform()
		{
			if (Time.timeSinceLevelLoad > _timeToForceDeath)
				RequestTransition(_puppetDeathState);
			
			Mover.RequestDisable();

			transform.parent.SetPositionAndRotation(
				Mover.Model.position, Mover.Model.rotation);

			if (_isRegainedBalance && !IsPuppetAnimPlaying)
			{
				if (TargetFinder.IsHasTarget)
					RequestTransition(ChaseState);
				else
					RequestTransition(IdleState);
			}
		}

		public override void Exit()
		{
			_isRegainedBalance = false;
			
			if (NextState != _puppetDeathState)
			{
				KickReceiver.SetReadyToKick();
				PuppetMasterHandler.SetMode(PuppetMaster.Mode.Disabled);
				Mover.Model.SetParent(transform.parent, true);
				PuppetMasterHandler.PuppetTransform.SetParent(transform.parent, true);
			}
			
			PuppetMasterHandler.OnRegainBalance -= OnRegainBalance;
			Health.OnTakeDamage -= OnTakeDamage;
			Health.OnZeroHealth -= OnDead;
			IsReadyToTransit = false;
		}
	}
}