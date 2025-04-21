using System.Collections;
using FAS.Players;
using UnityEngine;
using Zenject;

namespace FAS.FakePlayers.States
{
	public abstract class SkillCastState : FakePlayerState
	{
		[SerializeField] private float _cooldown = 3f;
		
		[Inject] private PlayerSpeedBoost _speedBoost;
		
		public bool IsCooldownActive { get; private set; }
		
		public override void Enter()
		{
			DamageReceiver.DisableDamageableColliders();
			StartCoroutine(Cooldown());
		}

		private IEnumerator Cooldown()
		{
			IsCooldownActive = true;
			yield return new WaitForSeconds(_cooldown);
			IsCooldownActive = false;
		}
		
		public override void Perform()
		{
			_speedBoost.RequestDisable();
		}
		
		public override void Exit()
		{
			base.Exit();
			DamageReceiver.EnableDamageableColliders();
		}
	}
}