using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace FAS.Zombies
{
	public class ZombieTargetFinder : MonoBehaviour
	{
		[SerializeField] private float _tryChangeTargetCooldown = 1f;
		[SerializeField] private float _loseTargetDelay = 3f;
		[SerializeField] private bool _isNeededEyesContact;
		[Tooltip("Obstacle Layers - For Line Cast Only")]
		[SerializeField] private LayerMask _obstacleLayers;

		[Inject] private BonesHolder _bonesHolder;

		private readonly HashSet<ITarget> _targets = new();
		
		private float _nextTryChangeTargetTime;
		private float _nextEyeContactCheckTime;
		private float _timeToLoseTarget;

		private bool _loseTargetRequested;

		public ITarget CurrentTarget { get; private set; }
		
		public bool IsHasTarget { get; private set; }
		
		private const float CHECK_EYE_CONTACT_INTERVAL = 1f;

		private void OnTriggerEnter(Collider other)
		{
			if (other.TryGetComponent(out ITarget target) && target.Type != TargetType.Zombie)
			{
				_targets.Add(target);

				if (!IsHasTarget)
				{
					CurrentTarget = target;

					if (_isNeededEyesContact)
					{
						IsHasTarget = IsHasEyesContact(CurrentTarget.HeadPosition);
						_nextEyeContactCheckTime = Time.timeSinceLevelLoad + CHECK_EYE_CONTACT_INTERVAL;
					}
					else
					{
						IsHasTarget = true;
					}

					_loseTargetRequested = false;
				}
			}
		}

		private void OnTriggerExit(Collider other)
		{
			if (other.TryGetComponent(out ITarget target))
			{
				_targets.Remove(target);
				
				if (CurrentTarget == target)
					RequestLoseTarget();
			}
		}

		private void RequestLoseTarget()
		{
			_timeToLoseTarget = _loseTargetDelay + Time.timeSinceLevelLoad;
			_loseTargetRequested = true;
		}

		public Vector3 GetCurrentTargetPosition()
		{
			return CurrentTarget.Position;
		}

		public void LoseTarget()
		{
			_loseTargetRequested = false;

			if (!TryChangeTarget())
			{
				CurrentTarget = null;
				IsHasTarget = false;
			}
		}

		private bool TryChangeTarget()
		{
			if (_targets.Count == 0)
				return false;

			ITarget closestTarget = null;
			var closestDistanceSqr = float.MaxValue;
			var currentPos = transform.position;

			foreach (var target in _targets)
			{
				if (_isNeededEyesContact && !IsHasEyesContact(target.HeadPosition))
					continue;

				var distanceSqr = (target.Position - currentPos).sqrMagnitude;

				if (distanceSqr < closestDistanceSqr)
				{
					closestDistanceSqr = distanceSqr;
					closestTarget = target;
				}
			}

			CurrentTarget = closestTarget;
			_nextTryChangeTargetTime = Time.timeSinceLevelLoad + _tryChangeTargetCooldown;
			IsHasTarget = CurrentTarget != null;
			
			return CurrentTarget != null;
		}

		private bool IsHasEyesContact(Vector3 targetPosition)
			=> !Physics.Linecast(_bonesHolder.Head.position, targetPosition, _obstacleLayers);

		private void Update()
		{
			if (CurrentTarget != null)
			{
				if (_targets.Contains(CurrentTarget) && _isNeededEyesContact
				                                     && Time.timeSinceLevelLoad > _nextEyeContactCheckTime)
				{
					IsHasTarget = IsHasEyesContact(CurrentTarget.HeadPosition);
					_loseTargetRequested = !IsHasTarget;
					_nextEyeContactCheckTime = Time.timeSinceLevelLoad + CHECK_EYE_CONTACT_INTERVAL;	
				}

				if (_loseTargetRequested && Time.timeSinceLevelLoad > _timeToLoseTarget)
					LoseTarget();
			}
			else if (_targets.Count > 0 && Time.timeSinceLevelLoad > _nextTryChangeTargetTime)
			{
				TryChangeTarget();
			}
		}

#if UNITY_EDITOR
		[Header("Debug")]
		[SerializeField] private bool _showGizmos;

		private void OnDrawGizmos()
		{
			if (_showGizmos && CurrentTarget != null)
			{
				Gizmos.color = IsHasEyesContact(CurrentTarget.HeadPosition) ? Color.green : Color.red;
				Gizmos.DrawLine(_bonesHolder.Head.position, CurrentTarget.HeadPosition);
			}
		}
#endif
	}
}