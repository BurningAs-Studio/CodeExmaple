using UnityEngine;

namespace FAS.Players
{
	public class PlayerRotator : MonoBehaviour
	{
		[SerializeField] private float _rotationSpeed = 20f;

		private bool _isSmoothRotateToRequested;
		private bool _isRotateToRequested;
		
		private Vector3 _smoothRotateToRequestVector;
		private float _rotateToRequestAngle;

		public void RequestRotateTo(float angle)
		{
			_rotateToRequestAngle = angle;
			_isRotateToRequested = true;
		}

		public void RequestSmoothRotateTo(Vector3 direction)
		{
			_smoothRotateToRequestVector = direction;
			_isSmoothRotateToRequested = true;
		}
		
		private void RotateTo(float angle)
		{
			transform.rotation = Quaternion.Euler(0f, angle, 0f);
		}

		private void SmoothRotateTo(Vector3 direction)
		{
			if (direction != Vector3.zero)
			{
				var targetRotation = Quaternion.LookRotation(direction, Vector3.up);
				transform.rotation = Quaternion.Slerp(
					transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
			}
		}
        
		public void LookAt(Vector3 position)
		{
			var directionToTarget = transform.position - position;
			var targetRotation = Quaternion.LookRotation(directionToTarget, Vector3.up);
			transform.rotation = targetRotation;

			var fixedAngles = transform.eulerAngles;
			fixedAngles.x = 0;
			fixedAngles.z = 0;
			transform.eulerAngles = fixedAngles;
		}

		private void Update()
		{
			if (_isRotateToRequested)
				RotateTo(_rotateToRequestAngle);
			else if (_isSmoothRotateToRequested)
				SmoothRotateTo(_smoothRotateToRequestVector);
			
			_isSmoothRotateToRequested = false;
			_isRotateToRequested = false;
		}
	}
}