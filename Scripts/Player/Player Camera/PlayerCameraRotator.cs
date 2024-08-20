using Cinemachine;
using UnityEngine;
using Zenject;
using System;

namespace PetWorld.Player
{
	public class PlayerCameraRotator : MonoBehaviour
	{
		[SerializeField] private float _autoRotateSpeed = 1f;
		
		[Header("Follow Camera")]
		[SerializeField] private CinemachineFreeLook _playerFollowCamera;
		[SerializeField] private float _followRotationSpeed = 3f;

		[Header("Aim Camera")]
		[SerializeField] private CinemachineVirtualCamera _aimCamera;
		[SerializeField] private Transform _verticalTarget;
		[SerializeField] private float _aimRotationSpeed = 3f;
		[SerializeField] private float _minVerticalAngle = -15f;
		[SerializeField] private float _maxVerticalAngle = 15f;
		
		[Inject] [NonSerialized] private CameraInputPanel _cameraInput;
		[Inject] [NonSerialized] private Transform _transform;
		
		private Transform _currentTarget;

		private float _currentFollowRotationSpeed;
		private float _currentHorizontalRotation;
		private float _currentAimRotationSpeed;
		private float _currentVerticalRotation;

		private bool _isAutoRotation;
		
		private void Awake()
		{
			_currentFollowRotationSpeed = _followRotationSpeed;
			_currentAimRotationSpeed = _aimRotationSpeed;
			
#if UNITY_EDITOR
			_currentFollowRotationSpeed *= 2;
			_currentAimRotationSpeed *= 2;
#endif
			CameraToPlayerRotation();
		}

		public void CameraToPlayerRotation()
		{
			_currentHorizontalRotation = _transform.eulerAngles.y;
			_currentVerticalRotation = _transform.eulerAngles.x;
		}
		
		public void ResetRotation()
		{
			_currentHorizontalRotation = 0;
			_currentVerticalRotation = 0;
		}
		
		public void StartAutoRotateToTarget(Transform target)
		{
			_currentTarget = target;
			_isAutoRotation = true;
		}
		
		public void StopAutoRotateToTarget()
		{
			_currentVerticalRotation = Mathf.Clamp(_transform.eulerAngles.x, _minVerticalAngle, _maxVerticalAngle);
			_currentHorizontalRotation = _transform.eulerAngles.y;
			_currentVerticalRotation = _transform.eulerAngles.x;

			_currentTarget = null;
			_isAutoRotation = false;
		}
		
		private void AutoRotation()
		{
			var targetDirection = _currentTarget.position - _transform.position;

			var targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);
			_transform.rotation = Quaternion.RotateTowards(
				_transform.rotation, targetRotation, _autoRotateSpeed * Time.deltaTime);
		}

		private void ManualRotationProcess()
		{
			float horizontalRotation;
			float verticalRotation;
			
			if (_aimCamera.enabled)
			{
				horizontalRotation = _cameraInput.CurrentInputVector.x * _currentAimRotationSpeed * Time.deltaTime;
				verticalRotation = -_cameraInput.CurrentInputVector.y * _currentAimRotationSpeed * Time.deltaTime;
			}
			else
			{
				horizontalRotation = _cameraInput.CurrentInputVector.x * _currentFollowRotationSpeed * Time.deltaTime;
				verticalRotation = -_cameraInput.CurrentInputVector.y * _currentFollowRotationSpeed * Time.deltaTime;
			}

			_currentHorizontalRotation += horizontalRotation;
			_currentVerticalRotation = Mathf.Clamp(
				_currentVerticalRotation += verticalRotation, _minVerticalAngle, _maxVerticalAngle);

			if (_aimCamera.enabled)
				RotateAimCamera();
			
			RotateFollowCamera();
		}
		
		private void RotateFollowCamera()
		{
			var currentVerticalRotationNormalized = Mathf.InverseLerp(
				_minVerticalAngle, _maxVerticalAngle, _currentVerticalRotation);
					
			_playerFollowCamera.m_XAxis.Value = _currentHorizontalRotation;
			_playerFollowCamera.m_YAxis.Value = currentVerticalRotationNormalized;  
		}
		
		private void RotateAimCamera()
		{
			_transform.rotation = Quaternion.Euler(0.0f, _currentHorizontalRotation, 0.0f);
			_verticalTarget.localRotation = Quaternion.Euler(_currentVerticalRotation, 0, 0);
		}
		
		private void LateUpdate()
		{
			if (_isAutoRotation)
				AutoRotation();
			else if (_cameraInput.IsInputProcess)
				ManualRotationProcess();
		}
	}
}