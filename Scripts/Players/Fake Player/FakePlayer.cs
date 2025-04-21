using FAS.Players.Animations;
using UnityEngine.AI;
using UnityEngine;
using Zenject;

namespace FAS.FakePlayers
{
	public class FakePlayer : MonoBehaviour, ITargetFakePlayer, IGroundChecker
	{
		[Inject(Id = BonesSkeletonType.Main)] private BonesHolder _mainSkeletonBones;
		[Inject] private FakePlayerStateMachine _stateMachine;
		[Inject] private FakePlayerBehaviourInfo _info;
		[Inject] private DamageReceiver _damageReceiver;
		[Inject] private JumpLayer _jumpAnimLayer;
		[Inject] private NavMeshAgent _agent;

		public FakePlayerBehaviourInfo Info => _info;
		
		public TargetType Type => TargetType.FakePlayer;
		
		public Vector3 HeadPosition => _mainSkeletonBones.Head.position;
		public Vector3 Position => transform.position;

		public bool IsGrounded => !_agent.isOnOffMeshLink;
		
		private void Start()
		{
			_stateMachine.Initialize();
		}

		private void Update()
		{
			_jumpAnimLayer.IsGrounded(IsGrounded);
		}

		public void TakeDamage(float damage, DamageReceiver damageDealer)
			=> _damageReceiver.TryTakeDamage(damage, BodyPart.Other, damageDealer);
	}
}
