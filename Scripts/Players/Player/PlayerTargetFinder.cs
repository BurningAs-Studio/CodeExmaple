using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace FAS.Players
{
	public class PlayerTargetFinder : MonoBehaviour, IOutlineHandler
	{
		[SerializeField] private float _inputThreshold = 0.5f;
		[SerializeField] private float _switchCooldown = 0.3f;
		[SerializeField] private LayerMask _obstacleLayers;

		[Inject (Id = BonesSkeletonType.Main)] private BonesHolder _bones;
		[Inject] private IReadOnlyPlayerCameraInputPanel _inputPanel;
		[Inject] private CamerasSwitcher _camerasSwitcher;
		[Inject] private Camera _mainCamera;

		private readonly HashSet<ITarget> _targets = new();

		private IOutlineEventsSender _currentOutlineEventsSender;

		private float _nextSwitchAvailableTime;

		public ITarget CurrentTarget { get; private set; }

		public bool IsHasTarget => CurrentTarget != null;

		private void OnTriggerEnter(Collider other)
		{
			if (other.TryGetComponent(out ITarget target))
				_targets.Add(target);
		}

		private void OnTriggerExit(Collider other)
		{
			if (other.TryGetComponent(out ITarget target))
			{
				_targets.Remove(target);

				if (CurrentTarget == target)
					LoseTarget();
			}
		}

		private bool IsTargetVisible(ITarget target)
			=> !Physics.Linecast(_bones.Head.position, target.HeadPosition, _obstacleLayers);

		private ITarget GetClosestTargetInView(Vector3 forward)
		{
			ITarget closest = null;
			var minAngle = float.MaxValue;

			foreach (var target in _targets)
			{
				if (IsTargetVisible(target))
				{
					var toTarget = target.Position - transform.position;
					toTarget.y = 0f;

					var angle = Vector3.Angle(forward, toTarget);

					if (closest == null || angle < minAngle)
					{
						minAngle = angle;
						closest = target;
					}
				}
			}

			return closest;
		}

		private ITarget GetTargetInDirection(float inputX, ITarget currentTarget)
		{
			ITarget bestTarget = null;
			float bestAngle = float.MaxValue;

			if (!Mathf.Approximately(inputX, 0f))
			{
				Vector3 currentDir = currentTarget.Position - transform.position;
				currentDir.y = 0f;
				currentDir.Normalize();

				int desiredSign = inputX > 0 ? 1 : -1;

				foreach (var target in _targets)
				{
					if (target != currentTarget)
					{
						if (IsTargetVisible(target))
						{
							Vector3 toTarget = target.Position - transform.position;
							toTarget.y = 0f;
							toTarget.Normalize();

							float angle = Vector3.SignedAngle(currentDir, toTarget, Vector3.up);
							float absAngle = Mathf.Abs(angle);

							if (Mathf.Sign(angle) == desiredSign)
							{
								if (absAngle < bestAngle)
								{
									bestAngle = absAngle;
									bestTarget = target;
								}
							}
						}
					}
				}
			}

			return bestTarget;
		}

		private void TrySetClosestTarget()
		{
			Vector3 forward = _mainCamera.transform.forward;
			forward.y = 0f;
			CurrentTarget = GetClosestTargetInView(forward);
		}

		private void TrySwitchTarget()
		{
			bool isInputStrongEnough = Mathf.Abs(_inputPanel.CurrentInputVector.x) > _inputThreshold;
			bool isCooldownFinished = Time.timeSinceLevelLoad > _nextSwitchAvailableTime;
			bool isPressed = _inputPanel.IsInputProcess;

			if (isInputStrongEnough && isCooldownFinished && isPressed)
			{
				ITarget newTarget = GetTargetInDirection(_inputPanel.CurrentInputVector.x, CurrentTarget);

				if (newTarget != null)
				{
					CurrentTarget = newTarget;
					_nextSwitchAvailableTime = Time.timeSinceLevelLoad + _switchCooldown;
				}
			}
		}
		
		public void ProcessOutline(Vector3 origin, Vector3 targetPosition)
		{
			_currentOutlineEventsSender?.TryDisableOutline();

			if (Physics.Raycast(origin, (targetPosition - origin).normalized, out var hit, _obstacleLayers))
			{
				if (hit.transform.TryGetComponent(out IOutlineEventsSender outlineEventsSender))
				{
					_currentOutlineEventsSender = outlineEventsSender;
					_currentOutlineEventsSender.TryEnableOutline();
				}
			}
		}

		public void LoseTarget()
		{
			if (_currentOutlineEventsSender != null)
			{
				_currentOutlineEventsSender?.TryDisableOutline();
				_currentOutlineEventsSender = null;
			}

			CurrentTarget = null;
		}

		private bool _isFindClosestTargetRequested;

		public void RequestFindClosestTarget() => _isFindClosestTargetRequested = true;
		
		private void Update()
		{
			if (_targets.Count > 0 && _camerasSwitcher.CurrentCamera == VirtualCameraType.Aim)
			{
				if (_isFindClosestTargetRequested)
				{
					if (IsHasTarget)
					{
						if (IsTargetVisible(CurrentTarget))
							TrySwitchTarget();
						else
							LoseTarget();
					}
					else
					{
						TrySetClosestTarget();
					}

					if (IsHasTarget)
						ProcessOutline(_bones.Head.position, CurrentTarget.HeadPosition);		
				}
			}
			else if (IsHasTarget)
			{
				LoseTarget();
			}

			_isFindClosestTargetRequested = false;
		}


#if UNITY_EDITOR
		[Header("DEBUG")]
		[SerializeField] private bool _showGizmos = true;
		
		private void OnDrawGizmos()
		{
			if (_showGizmos && Application.isPlaying)
			{
				var origin = _bones.Head.position;

				foreach (var target in _targets)
				{
					if (target != null)
					{
						var destination = target.HeadPosition;

						var visible = !Physics.Linecast(origin, destination, _obstacleLayers);
						Gizmos.color = visible ? Color.green : Color.red;
						Gizmos.DrawLine(origin, destination);

						Gizmos.color = target == CurrentTarget ? Color.yellow : Color.gray;
						Gizmos.DrawSphere(destination, 0.1f);
					}
				}	
			}
		}
#endif
	}
}
