using Cinemachine;
using UnityEngine;
using Zenject;

namespace FAS.Players
{
	public class PlayerCameraRotator : MonoBehaviour
	{
		[SerializeField] private Transform _verticalTarget;
		[SerializeField] private float _minVerticalAngle = -15f;
		[SerializeField] private float _maxVerticalAngle = 15f;
		[SerializeField] private float _breakInputThreshold = 0.1f;

		[Inject(Id = VirtualCameraType.Aim)] private CinemachineVirtualCamera _aimCamera;
		[Inject] private IReadOnlyPlayerCameraInputPanel _cameraInput;
		[Inject] private CinemachineFreeLook _followCamera;

		private Vector3 _lookAtRequestTarget;

		private float _targetHorizontalRotation;
		private float _currentVerticalRotation;

		private bool _isDisableRequested;
		private bool _isLookAtRequested;

		private const float MAX_ROTATE_LENGTH = 360f;
		private const float AUTO_AIM_SPEED = 360f;

		public float CurrentHorizontalRotation { get; private set; }

		private void Awake()
		{
			CurrentHorizontalRotation = transform.eulerAngles.y;
			_currentVerticalRotation = transform.eulerAngles.x;
		}

		public void RequestDisable() => _isDisableRequested = true;

		public void RequestLookAt(Vector3 target)
		{
			_lookAtRequestTarget = target;
			_isLookAtRequested = true;
		}

		public void ResetRotation()
		{
			CurrentHorizontalRotation = 0f;
			_currentVerticalRotation = 0f;
		}

		private void ManualRotation()
		{
			var horizontal = _cameraInput.CurrentInputVector.x * Settings.CurrentCameraSensitivity * Time.deltaTime;
			var vertical = -_cameraInput.CurrentInputVector.y * Settings.CurrentCameraSensitivity * Time.deltaTime;

			CurrentHorizontalRotation += horizontal;
			_currentVerticalRotation = Mathf.Clamp(
				_currentVerticalRotation + vertical, _minVerticalAngle, _maxVerticalAngle);

			ApplyRotationToCameras();
		}

		private void LookAt()
		{
			var direction = _lookAtRequestTarget - _aimCamera.transform.position;

			var directionXZ = direction;
			directionXZ.y = 0f;

			if (directionXZ.sqrMagnitude > 0.0001f)
			{
				var forward = _aimCamera.transform.forward;
				forward.y = 0f;

				var targetYaw =
					Mathf.Repeat(CurrentHorizontalRotation + Vector3.SignedAngle(forward.normalized,
						directionXZ.normalized, Vector3.up), MAX_ROTATE_LENGTH);

				CurrentHorizontalRotation = Mathf.MoveTowardsAngle(
					CurrentHorizontalRotation, targetYaw,
					Settings.CurrentCameraSensitivity * Time.deltaTime * AUTO_AIM_SPEED);
			}

			var pitch = -Mathf.Atan2(direction.y, directionXZ.magnitude) * Mathf.Rad2Deg;
			var targetPitch = Mathf.Clamp(pitch, _minVerticalAngle, _maxVerticalAngle);

			_currentVerticalRotation = Mathf.MoveTowards(
				_currentVerticalRotation, targetPitch,
				Settings.CurrentCameraSensitivity * Time.deltaTime * AUTO_AIM_SPEED);

			ApplyRotationToCameras();
		}


		private void ApplyRotationToCameras()
		{
			var yNormalized = Mathf.InverseLerp(_minVerticalAngle, _maxVerticalAngle, _currentVerticalRotation);

			_followCamera.m_XAxis.Value = CurrentHorizontalRotation;
			_followCamera.m_YAxis.Value = yNormalized;

			_verticalTarget.localRotation = Quaternion.Euler(_currentVerticalRotation, 0f, 0f);
		}

		private void LateUpdate()
		{
			if (_isDisableRequested)
			{
				_isDisableRequested = false;
			}
			else
			{
				if (_isLookAtRequested)
					LookAt();
				else if (_cameraInput.IsInputProcess)
					ManualRotation();	
			}

			_isLookAtRequested = false;
		}
	}
}