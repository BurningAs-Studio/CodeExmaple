using UnityEngine;

namespace FAS.Players
{
	public class PlayerJump : MonoBehaviour, IReadOnlyPlayerJump
	{
		[SerializeField] private float _jumpHeight = 1.2f;
		[SerializeField] private float _gravity = -15.0f;
		[SerializeField] private float _jumpTimeout = 0.50f;
		[SerializeField] private float _jumpDelay = 0.1f;

		private float _verticalVelocity;
		private float _jumpTimeoutDelta;
		private float _fallTimeoutDelta;
		private float _jumpDelayTimer;
		
		private bool _isWaitingForJump;

		private readonly float _terminalVelocity = 53.0f;


		public bool TryJump(bool isJumpRequested)
		{
			if (_verticalVelocity < 0.0f)
				_verticalVelocity = -2f;

			if (isJumpRequested && _jumpTimeoutDelta <= 0.0f)
			{
				if (!_isWaitingForJump)
				{
					_isWaitingForJump = true;
					_jumpDelayTimer = _jumpDelay;	
				}
				return true;
			}

			if (_jumpTimeoutDelta >= 0.0f)
				_jumpTimeoutDelta -= Time.deltaTime;

			return false;
		}

		private void Jump()
		{
			_verticalVelocity = Mathf.Sqrt(_jumpHeight * -2f * _gravity);
		}

		public void FreeFall()
		{
			_jumpTimeoutDelta = _jumpTimeout;

			if (_fallTimeoutDelta >= 0.0f)
				_fallTimeoutDelta -= Time.deltaTime;
		}

		public void CheckAndRestVelocity()
		{
			if (_verticalVelocity < 0)
				_verticalVelocity = -2f;
		}

		public void ApplyGravity()
		{
			if (_verticalVelocity < _terminalVelocity)
				_verticalVelocity += _gravity * Time.deltaTime;
		}

		public Vector3 GetJumpVector()
		{
			return new Vector3(0.0f, _verticalVelocity, 0.0f);
		}

		private void Update()
		{
			if (_isWaitingForJump)
			{
				_jumpDelayTimer -= Time.deltaTime;

				if (_jumpDelayTimer < 0)
				{
					Jump();
					_isWaitingForJump = false;	
				}
			}
		}
	}
}