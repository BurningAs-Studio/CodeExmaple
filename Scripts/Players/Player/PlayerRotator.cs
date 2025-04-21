using UnityEngine;
using Zenject;

namespace FAS.Players
{
	public class PlayerRotator : MonoBehaviour
	{
		[SerializeField] private float _rotationSpeed = 20f;
		[SerializeField] private float _bodyRotationSpeed = 20f;
		
		[Inject (Id = CharacterTransformType.Body)] private Transform _body;
		[Inject]private CamerasSwitcher _camerasSwitcher;
		
		private Quaternion _rotateRequest;
		
		private Vector3 _smoothRotateBodyHorizontalRequest;
		private Vector3 _smoothRotateFullRequest;
		
		private float _rotateHorizontalRequest;
		
		private bool _isSmoothRotateBodyHorizontalRequested;
		private bool _isSmoothRotateFullRequested;
		private bool _isRotateHorizontalRequested;
		private bool _isRotateRequested;

		public float GetBodyAngleToTarget(Vector3 targetPosition)
		{
			var direction = targetPosition - _body.position;
			direction.y = 0f;

			return Quaternion.Angle(_body.rotation, Quaternion.LookRotation(direction));
		}

		public void RequestSmoothRotateBodyHorizontal(Vector3 angles)
		{
			_smoothRotateBodyHorizontalRequest = angles;
			_isSmoothRotateBodyHorizontalRequested = true;
		}

		public void RequestRotateHorizontal(float angle)
		{
			_rotateHorizontalRequest = angle;
			_isRotateHorizontalRequested = true;
		}

		public void RequestRotate(Quaternion target)
		{
			_rotateRequest = target;
			_isRotateRequested = true;
		}

		public void RequestSmoothRotateFull(Vector3 direction)
		{
			_smoothRotateFullRequest = direction;
			_isSmoothRotateFullRequested = true;
		}
		
		public void LookAt(Vector3 targetPosition)
		{
			var directionToTarget = targetPosition - transform.position;
			var targetRotation = Quaternion.LookRotation(directionToTarget, Vector3.up);
			transform.rotation = targetRotation;

			var fixedAngles = transform.eulerAngles;
			fixedAngles.x = 0;
			fixedAngles.z = 0;
			transform.eulerAngles = fixedAngles;
		}

		public void ForceRotateTo(float angle) => RotateTo(angle);
		
		private void RotateTo(float angle) => transform.rotation = Quaternion.Euler(0f, angle, 0f);
		
		private void RotateTo(Quaternion target) => transform.rotation = target;

		private void SmoothRotateFull(Transform target, Vector3 direction, float speed)
		{
			if (direction.sqrMagnitude > 0.0001f)
			{
				var targetRotation = Quaternion.LookRotation(direction, Vector3.up);
				target.rotation = Quaternion.Slerp(
					target.rotation, targetRotation, speed * Time.deltaTime);
			}
		}
		
		private void SmoothRotateHorizontal(Transform target, Vector3 direction, float speed)
		{
			if (direction.sqrMagnitude > 0.0001f)
			{
				direction.y = 0f;
				
				var targetRotation = Quaternion.LookRotation(direction, Vector3.up);
				var smoothedY = Mathf.LerpAngle(
					target.eulerAngles.y, targetRotation.eulerAngles.y, speed * Time.deltaTime);
				
				target.rotation = Quaternion.Euler(0f, smoothedY, 0f);
			}
		}

		private void SmoothResetBodyRotation()
		{
			_body.localRotation = Quaternion.Slerp(
				_body.localRotation, Quaternion.identity, _bodyRotationSpeed * Time.deltaTime);
		}

		private void Update()
		{
			if (_camerasSwitcher.IsCameraRestarted)
			{
				if (_isRotateRequested)
					RotateTo(_rotateRequest);
				else if (_isRotateHorizontalRequested)
					RotateTo(_rotateHorizontalRequest);
				else if (_isSmoothRotateFullRequested)
					SmoothRotateFull(transform, _smoothRotateFullRequest, _rotationSpeed);

				if (_isSmoothRotateBodyHorizontalRequested)
					SmoothRotateHorizontal(_body, _smoothRotateBodyHorizontalRequest, _bodyRotationSpeed);
				else
					SmoothResetBodyRotation();	
			}
			
			_isRotateRequested = false;
			_isSmoothRotateFullRequested = false;
			_isRotateHorizontalRequested = false;
			_isSmoothRotateBodyHorizontalRequested = false;
		}
	}
}