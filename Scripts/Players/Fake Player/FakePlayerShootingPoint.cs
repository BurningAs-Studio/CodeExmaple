using Random = UnityEngine.Random;
using FAS.Zombies;
using UnityEngine;
using Zenject;

namespace FAS.FakePlayers
{
	public class FakePlayerShootingPoint : MonoBehaviour
	{
		[SerializeField] private float _moveSpeed = 5f;
		[SerializeField] private float _minDistanceToSwitchPoint = 0.1f;

		[Inject] private FakePlayerTargetFinder _targetFinder;
		
		private Vector3 _currentTargetPosition;
		
		private float _nextUpdateTargetTime;

		private bool _isNewTarget;
		
		private const float UPDATE_TARGET_RATE = 0.25f;
		
		public BodyPart CurrentClosestBodyPart { get; private set; } = BodyPart.Other;
		
		private void OnEnable()
		{
			_targetFinder.OnTargetChanged += OnTargetChanged;
		}

		private void OnDisable()
		{
			_targetFinder.OnTargetChanged -= OnTargetChanged;
		}

		private void OnTargetChanged() => _isNewTarget = true;
		
		private void SetInitialTargetPosition(ITargetZombie zombie)
		{
			_currentTargetPosition = GetRandomPointBetween(zombie.HeadPosition, zombie.Position);

			var targetPosition = zombie.Position;
			targetPosition.y = _currentTargetPosition.y;
			transform.position = targetPosition;

			_nextUpdateTargetTime = Time.timeSinceLevelLoad + UPDATE_TARGET_RATE;
			_isNewTarget = false;
		}
		
		private void UpdateTarget(ITargetZombie zombie)
		{
			_nextUpdateTargetTime = Time.timeSinceLevelLoad + UPDATE_TARGET_RATE;

			if (Mathf.Abs(transform.position.y - _currentTargetPosition.y) < _minDistanceToSwitchPoint)
				_currentTargetPosition = GetRandomPointBetween(zombie.HeadPosition, zombie.Position);

			UpdateCurrentClosestBodyPart(zombie);
		}
		
		private Vector3 GetRandomPointBetween(Vector3 head, Vector3 body)
		{
			var targetPosition = body;
			targetPosition.y = Random.Range(body.y, head.y);
			return targetPosition;
		}

		private void UpdateCurrentClosestBodyPart(ITargetZombie zombie)
		{
			var posXZ = new Vector2(transform.position.x, transform.position.z);
			var headXZ = new Vector2(zombie.HeadPosition.x, zombie.HeadPosition.z);
			var bodyXZ = new Vector2(zombie.Position.x, zombie.Position.z);

			var distanceToHeadSqr = (posXZ - headXZ).sqrMagnitude;
			var distanceToBodySqr = (posXZ - bodyXZ).sqrMagnitude;

			CurrentClosestBodyPart = distanceToHeadSqr < distanceToBodySqr ? BodyPart.Head : BodyPart.Other;
		}

		private void MoveToTarget(ITargetZombie zombie)
		{
			var targetPosition = zombie.Position;
			targetPosition.y = Mathf.MoveTowards(
				transform.position.y, _currentTargetPosition.y, _moveSpeed * Time.deltaTime);

			transform.position = targetPosition;
		}
		
		private void Update()
		{
			if (_targetFinder.IsHasTargetZombie)
			{
				var zombie = _targetFinder.CurrentTargetZombie;

				if (_isNewTarget)
				{
					SetInitialTargetPosition(zombie);
				}
				else
				{
					if (Time.timeSinceLevelLoad > _nextUpdateTargetTime)
						UpdateTarget(zombie);

					MoveToTarget(zombie);
				}
			}
		}
	}
}