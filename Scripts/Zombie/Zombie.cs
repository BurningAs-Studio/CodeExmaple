using UnityEngine;
using Zenject;

namespace FAS.Zombies
{
	public class Zombie : MonoBehaviour, ITargetZombie
	{
		[Inject] private ZombieStateMachine _stateMachine;
		[Inject] private DamageReceiver _damageReceiver;
		[Inject] private ZombieBehaviourInfo _info;
		[Inject] private BonesHolder _bonesHolder;

		public ZombieBehaviourInfo Info => _info;

		public TargetType Type => TargetType.Zombie;
		
		public Vector3 HeadPosition => _bonesHolder.Head.position;
		public Vector3 Position => transform.position;

		private void Start() => _stateMachine.Initialize();

		public void TakeDamage(float damage, DamageReceiver damageDealer)
			=> _damageReceiver.TryTakeDamage(damage, BodyPart.Other, damageDealer);
	}
}