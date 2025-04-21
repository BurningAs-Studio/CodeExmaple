using FAS.Players.Animations;
using UnityEngine.AI;
using FAS.Players;
using UnityEngine;
using Zenject;

namespace FAS.FakePlayers
{
	public class FakePlayerMover : MonoBehaviour, ISpeedMultiplier
	{
		[SerializeField] private float _maxMoveSpeed = 8f;
		[SerializeField] private float _movementLerpTime = 8f;
		[SerializeField] private float _decelerationTime = 15f;

		[Inject] private PlayerAnimator _animator;
		[Inject] private NavMeshAgent _agent;
		
		private NavMeshPath _navMeshPath;

		private Vector3 _transformMoveTargetPosition;
		private Vector3 _teleportTargetPosition;
		private Vector3 _navMeshTargetPosition;
		private Vector2 _currentVelocity;
		private Vector2 _targetVelocity;
		
		private float _currentSpeedMultiplier = 1f;
		private float _nextPathUpdateTime;
		private float _transformMoveSpeed;
		private float _nextCheckPathTime;
		
		private bool _isDisableNavMeshRequested;
		private bool _isTransformMoveThisFrame;
		private bool _isTransformMoveRequested;
		private bool _isDisableAllRequested;
		private bool _isTeleportRequested;
		
		public bool IsMultipliedThisFrame { get; private set; }
		
		public Vector2 CurrentVelocity => _currentVelocity;
		
		public float CurrentMaxSpeed => _maxMoveSpeed * _currentSpeedMultiplier;
		
		private const float MIN_REMAINING_DISTANCE = 0.1f;
		private const float PATH_UPDATE_INTERVAL = 0.2f;

		public bool IsProcessMovement => _agent.enabled && (_isTransformMoveThisFrame || _agent.pathPending
		                                 || _agent.remainingDistance > MIN_REMAINING_DISTANCE + _agent.stoppingDistance);
		public bool IsMovingToTarget { get; private set; }
		public bool IsMovedLastFrame { get; private set; }
		
		private void Awake()
		{
			_navMeshPath = new NavMeshPath();
			_agent.autoRepath = false;
		}

		public void RequestDisableNavMesh() => _isDisableNavMeshRequested = true;
		
		public void RequestDisableAll() => _isDisableAllRequested = true;
		
		public void SetStoppingDistance(float stoppingDistance) => _agent.stoppingDistance = stoppingDistance;

		public bool CalculatePath(Vector3 target)
		{
			IsMovingToTarget = false;

			if (_navMeshTargetPosition != target)
			{
				_navMeshTargetPosition = target;

				if (!_agent.CalculatePath(target, _navMeshPath))
					return IsMovingToTarget;
			}

			IsMovingToTarget = _navMeshPath.status == NavMeshPathStatus.PathComplete && _navMeshPath.corners.Length > 0;
			return IsMovingToTarget;
		}

		public void NavMeshMove(Vector3 target)
		{
			_agent.enabled = true;
			_agent.updatePosition = true;

			if (Time.time >= _nextPathUpdateTime)
			{
				_nextPathUpdateTime = Time.time + PATH_UPDATE_INTERVAL;

				if (CalculatePath(target))
					_agent.SetDestination(_navMeshTargetPosition);
				else if (_navMeshPath.corners != null && _navMeshPath.corners.Length > 0)
					_agent.SetDestination(_navMeshPath.corners[^1]);
				else
					_agent.ResetPath();
			}
		}
		
		public void TryStopMove()
		{
			if (_agent.enabled && !_isDisableAllRequested)
				_agent.SetDestination(transform.position);
		}

		public void RequestTeleport(Vector3 position)
		{
			_isTeleportRequested = true;
			_teleportTargetPosition = position;
		}

		public void RequestTransformMove(Vector3 target, float speed)
		{
			_isTransformMoveRequested = true;
			_transformMoveTargetPosition = target;
			_transformMoveSpeed = speed;
			
			_isTransformMoveThisFrame =
				Vector3.SqrMagnitude(transform.position - _transformMoveTargetPosition) > 0.1f;
		}
		
		public float GetSqrMagnitudeToTarget()
		{
			var distance = _agent.stoppingDistance + MIN_REMAINING_DISTANCE;
			return distance * distance;
		}
		
		private void Teleport()
		{
			_agent.enabled = false;
			transform.position = _teleportTargetPosition;
			_agent.enabled = true;
		}

		private void TransformMove()
		{
			transform.position = Vector3.MoveTowards(transform.position, 
				_transformMoveTargetPosition, _transformMoveSpeed * Time.deltaTime);
				
			_agent.updatePosition = false;
		}
		
		private void UpdateVelocity()
		{
			_targetVelocity = new Vector2(_agent.velocity.x, _agent.velocity.z);
			_currentVelocity = Vector2.Lerp(_currentVelocity, _targetVelocity, _movementLerpTime * Time.deltaTime);
		}

		public void SetSpeedMultiplier(float multiplier)
		{
			IsMultipliedThisFrame = true;
			_currentSpeedMultiplier = multiplier;
		}

		private void ResetSpeedMultiplier() => _currentSpeedMultiplier = 1;
		
		private void Update()
		{
			if (_isDisableAllRequested)
			{
				_agent.enabled = false;
				_isDisableAllRequested = false;
			}
			else
			{
				if (_isDisableNavMeshRequested)
					_agent.enabled = false;

				if (_isTeleportRequested)
				{
					Teleport();
				}
				else if (_isTransformMoveRequested)
				{
					TransformMove();
				}
				else if (!_isDisableNavMeshRequested)
				{
					_agent.enabled = true;
					_agent.updatePosition = true;
				}
			}

			UpdateVelocity();

			IsMovedLastFrame = IsProcessMovement;
			
			_isDisableNavMeshRequested = false;
			_isTransformMoveRequested = false;
			_isTransformMoveThisFrame = false;
			_isDisableAllRequested = false;
			_isTeleportRequested = false;
		}
		
		private void LateUpdate()
		{
			if (IsMultipliedThisFrame)
				IsMultipliedThisFrame = false;
			else
				ResetSpeedMultiplier();
		}
	}
}