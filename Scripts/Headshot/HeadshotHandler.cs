using UnityEngine;
using Zenject;

namespace FAS
{
	public abstract class HeadshotHandler : MonoBehaviour
	{
		[SerializeField] private float _headResetSpeed = 5f;
		[SerializeField] private float _maxHeadAngle = 125f;
		
		[Inject] private DamageReceiver _damageReceiver;

		protected Quaternion TargetRotation = Quaternion.identity;

		private Vector3 _backwardAngles;
		private Vector3 _forwardAngles;
		private Vector3 _rightAngles;
		private Vector3 _leftAngles;

		private bool _isActive;

		private void Awake()
		{
			_backwardAngles = new Vector3(-_maxHeadAngle, 0f, 0f);
			_forwardAngles = new Vector3(_maxHeadAngle, 0f, 0f);
			_rightAngles = new Vector3(0f, 0f, -_maxHeadAngle);
			_leftAngles = new Vector3(0f, 0f, _maxHeadAngle);
		}

		private void OnEnable()
		{
			_damageReceiver.OnTakeDamageInHead += OnHeadshot;
		}

		private void OnDisable()
		{
			_damageReceiver.OnTakeDamageInHead -= OnHeadshot;
		}

		protected virtual void OnHeadshot()
		{
			var attackDirection =
				(_damageReceiver.LastDamageDealer.transform.position - transform.position).normalized;

			var offsetEuler = DirectionHelper.GetHitDirection(transform.forward, attackDirection) switch
			{
				HitDirection.Front => _backwardAngles,
				HitDirection.Back => _forwardAngles,
				HitDirection.Left => _rightAngles,
				HitDirection.Right => _leftAngles,
				_ => Vector3.zero
			};

			var offsetRotation = Quaternion.Euler(offsetEuler);
			TargetRotation = Quaternion.identity * offsetRotation;

			_isActive = true;
		}

		protected abstract Quaternion GetHeadTargetRotation();
		
		protected virtual void RotateHead(Quaternion currentRotation)
			=> TargetRotation = Quaternion.Slerp(
				currentRotation, Quaternion.identity, Time.deltaTime * _headResetSpeed);

		private void Update()
		{
			if (_isActive)
			{
				var currentRotation = GetHeadTargetRotation();
				
				if (Quaternion.Angle(currentRotation, Quaternion.identity) > 0.1f)
					RotateHead(currentRotation);
				else
					_isActive = false;
			}
		}

	}
}