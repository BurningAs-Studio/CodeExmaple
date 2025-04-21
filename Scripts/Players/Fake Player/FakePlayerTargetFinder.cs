using System.Collections.Generic;
using System.Linq;
using FAS.Zombies;
using UnityEngine;
using Zenject;
using System;

namespace FAS.FakePlayers
{
	public class FakePlayerTargetFinder : MonoBehaviour
	{
		[Tooltip("Obstacle Layers - For Line Cast Only")]
		[SerializeField] private LayerMask _obstacleLayers;
		
		[Inject(Id = BonesSkeletonType.Main)] private BonesHolder _bonesHolder;
		[Inject] private ITargetFakePlayer _selfTarget;

		private float _nextTimeFindClosestZombie;
		
		public ITargetZombie CurrentTargetZombie { get; private set; }

		private readonly HashSet<ITargetZombie> _aggressiveZombies = new ();
		private readonly HashSet<ITargetZombie> _allZombies = new ();
		
		public bool IsHasTargetZombie => CurrentTargetZombie != null;

		public event Action OnTargetChanged;
		
		private const float FIND_CLOSEST_ZOMBIE_COOLDOWN = 1f;
		
		private void OnTriggerEnter(Collider other)
		{
			if (other.TryGetComponent(out ITargetZombie zombie))
			{
				_allZombies.Add(zombie);
			}
		}

		private void OnTriggerExit(Collider other)
		{
			if (other.TryGetComponent(out ITargetZombie zombie))
			{
				_allZombies.Remove(zombie);
				_aggressiveZombies.Remove(zombie);

				if (IsHasTargetZombie && CurrentTargetZombie == zombie)
					UpdateCurrentTargetZombie();
			}
		}

		private void UpdateCurrentTargetZombie()
		{
			if (_aggressiveZombies.Count > 0)
			{
				if (_aggressiveZombies.Count == 1)
				{
					var newTarget = _aggressiveZombies.First();

					if (CurrentTargetZombie != newTarget)
					{
						CurrentTargetZombie = newTarget;
						OnTargetChanged?.Invoke();
					}

					_nextTimeFindClosestZombie = Time.timeSinceLevelLoad + FIND_CLOSEST_ZOMBIE_COOLDOWN;
				}
				else
				{
					FindClosestZombie();
				}
			}
			else if (IsHasTargetZombie)
			{
				LoseCurrentTargetZombie();
			}
		}

		private void FindClosestZombie()
		{
			var previousTarget = CurrentTargetZombie;
			CurrentTargetZombie = null;

			var closestDistanceSqr = float.MaxValue;
			var currentPos = transform.position;

			foreach (var zombie in _aggressiveZombies)
			{
				if (IsHasEyesContact(zombie.HeadPosition))
				{
					var distanceSqr = (zombie.Position - currentPos).sqrMagnitude;

					if (distanceSqr < closestDistanceSqr)
					{
						closestDistanceSqr = distanceSqr;
						CurrentTargetZombie = zombie;
					}
				}
			}

			_nextTimeFindClosestZombie = Time.timeSinceLevelLoad + FIND_CLOSEST_ZOMBIE_COOLDOWN;

			if (CurrentTargetZombie != previousTarget)
				OnTargetChanged?.Invoke();

			if (!IsHasTargetZombie)
				LoseCurrentTargetZombie();
		}

		private void LoseCurrentTargetZombie()
		{
			CurrentTargetZombie = null;
		}
		
		private bool IsHasEyesContact(Vector3 targetPosition)
			=> !Physics.Linecast(_bonesHolder.Head.position, targetPosition, _obstacleLayers);
		
		private void Update()
		{
			if (_allZombies.Count > 0 && Time.timeSinceLevelLoad > _nextTimeFindClosestZombie)
			{
				_aggressiveZombies.Clear();

				foreach (var zombie in _allZombies)
					if (zombie.Info.IsHasTarget && zombie.Info.CurrentTarget == _selfTarget)
						_aggressiveZombies.Add(zombie);

				UpdateCurrentTargetZombie();
			}
			else if (IsHasTargetZombie && _allZombies.Count == 0)
			{
				LoseCurrentTargetZombie();
			}
		}

		
#if UNITY_EDITOR
		[Header("Debug")]
		[SerializeField] private bool _showGizmos;

		private void OnDrawGizmos()
		{
			if (_showGizmos && IsHasTargetZombie)
			{
				Gizmos.color = IsHasEyesContact(CurrentTargetZombie.HeadPosition) ? Color.green : Color.red;
				Gizmos.DrawLine(_bonesHolder.Head.position, CurrentTargetZombie.HeadPosition);
			}
		}
#endif
	}
}