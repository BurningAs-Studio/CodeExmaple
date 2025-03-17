using UnityEngine.AI;
using UnityEngine;
using Zenject;

namespace FAS.Zombies
{
	public class ZombieMover : MonoBehaviour
	{
		[Inject] private ZombieAnimator _animator;
		[Inject] private NavMeshAgent _agent;

		private NavMeshPath _navMeshPath;

		private Vector3 _transformMoveTargetPosition;
		private Vector3 _teleportTargetPosition;
		private Vector3 _navMeshTargetPosition;
		
		private float _transformMoveSpeed;
		private float _nextCheckPathTime;
		private float _nextPathUpdateTime;
		private bool _isRootMotionMovedLastFrame;
		private bool _isTransformMoveThisFrame;
		private bool _isTransformMoveRequested;
		private bool _isRootMotionRequested;
		private bool _isTeleportRequested;
		private bool _isMovedLastFrame;
		
		private const float MIN_REMAINING_DISTANCE = 0.1f;
		private const float PATH_UPDATE_INTERVAL = 0.2f;

		public bool IsProcessMovement => _isTransformMoveThisFrame
		                                 || _isRootMotionMovedLastFrame
		                                 || _agent.pathPending
		                                 || _agent.remainingDistance > MIN_REMAINING_DISTANCE + _agent.stoppingDistance;
		public bool IsMovingToTarget { get; private set; }
		public bool IsMovedLastFrame { get; private set; }
		
		private void Awake()
		{
			_navMeshPath = new NavMeshPath();
			_agent.autoRepath = false;
		}
		
		public void SetStoppingDistance(float stoppingDistance) => _agent.stoppingDistance = stoppingDistance;

		public void RequestEnableRootMotion() => _isRootMotionRequested = true;

		public bool CalculatePath(Vector3 target)
		{
			if (_navMeshTargetPosition != target)
			{
				_navMeshTargetPosition = target;
				
				if (_agent.CalculatePath(target, _navMeshPath))
				{
					if (_navMeshPath.status == NavMeshPathStatus.PathComplete)
						IsMovingToTarget = true;
					else if (_navMeshPath.status == NavMeshPathStatus.PathPartial)
						IsMovingToTarget = false;
				}
			}
			
			return IsMovingToTarget;
		}

		public void NavMeshMove(Vector3 target)
		{
			_agent.updatePosition = true;

			if (Time.time >= _nextPathUpdateTime)
			{
				_nextPathUpdateTime = Time.time + PATH_UPDATE_INTERVAL;
				_agent.SetDestination(CalculatePath(target) ? _navMeshTargetPosition : _navMeshPath.corners[^1]);
			}
		}

		
		public void StopMove() => _agent.SetDestination(transform.position);
		
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
		
		private void RootMotionMove()
		{
			_agent.speed = 0;
			transform.position += _animator.DeltaPosition;
			transform.rotation *= _animator.DeltaRotation;

			_isRootMotionMovedLastFrame = _animator.DeltaPosition != Vector3.zero;
		}
		
		private void Update()
		{
			if (_isTeleportRequested)
				Teleport();
			else if (_isTransformMoveRequested)
				TransformMove();
			else
				_agent.updatePosition = true;
			
			_isTransformMoveRequested = false;
			_isTeleportRequested = false;
		}
		
		private void OnAnimatorMove()
		{
			_isRootMotionMovedLastFrame = false;

			if (_isRootMotionRequested)
				RootMotionMove();
			else
				_agent.speed = _animator.Velocity.magnitude;
			
			_isRootMotionRequested = false;
		}
		
		private void LateUpdate()
		{
			_isTransformMoveThisFrame = false;
			_isMovedLastFrame = IsMovedLastFrame;
			IsMovedLastFrame = _isMovedLastFrame || IsProcessMovement;
		}
	}
}
