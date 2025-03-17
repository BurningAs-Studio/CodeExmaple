using System;
using Cinemachine;
using UnityEngine;
using Zenject;

namespace FAS.Players
{
	public class PlayerCameraRotator : MonoBehaviour
	{
		[SerializeField] private float _minVerticalAngle = -15f;
		[SerializeField] private float _maxVerticalAngle = 15f;
		[SerializeField] private float _followRotationSpeed = 3f;
		
		[Inject] private PlayerCameraInputPanel _cameraInput;
		[Inject] private CinemachineFreeLook _followCamera;
		
		public float CurrentHorizontalRotation { get; private set; }
		
		private float _currentFollowRotationSpeed;
		private float _currentVerticalRotation;
		
		private void Awake()
		{
			_currentFollowRotationSpeed = _followRotationSpeed;
			CameraToPlayerRotation();
		}

		private void OnEnable()
		{
		}

		private void CameraToPlayerRotation()
		{
			CurrentHorizontalRotation = transform.eulerAngles.y;
			_currentVerticalRotation = transform.eulerAngles.x;
		}
		
		public void ResetRotation()
		{
			CurrentHorizontalRotation = 0;
			_currentVerticalRotation = 0;
		}

		public void ManualRotation()
		{
			var horizontalRotation = _cameraInput.CurrentInputVector.x * _currentFollowRotationSpeed * Time.deltaTime;
			var verticalRotation = -_cameraInput.CurrentInputVector.y * _currentFollowRotationSpeed * Time.deltaTime;

			CurrentHorizontalRotation += horizontalRotation;
			_currentVerticalRotation = Mathf.Clamp(
				_currentVerticalRotation += verticalRotation, _minVerticalAngle, _maxVerticalAngle);
			
			RotateFollowCamera();
		}
		
		private void RotateFollowCamera()
		{
			var currentVerticalRotationNormalized = Mathf.InverseLerp(
				_minVerticalAngle, _maxVerticalAngle, _currentVerticalRotation);
					
			_followCamera.m_XAxis.Value = CurrentHorizontalRotation;
			_followCamera.m_YAxis.Value = currentVerticalRotationNormalized;  
		}
	}
}