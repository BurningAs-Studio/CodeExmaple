using FAS.Players.Animations;
using UnityEngine;
using Zenject;

namespace FAS.Players
{
	public class PlayerJump : MonoBehaviour, IReadOnlyPlayerJump
	{
		[SerializeField] private float _jumpHeight = 1.2f;
		[SerializeField] private float _gravity = -15.0f;
		[SerializeField] private float _jumpTimeout = 0.50f;
		[SerializeField] private float _jumpDelay = 0.1f;
		
		[Inject] private IGroundChecker _groundChecker;
		[Inject] private JumpLayer _jumpAnimLayer;

		private float _verticalVelocity;
		private float _jumpTimeoutDelta;
		private float _fallTimeoutDelta;
		private float _jumpDelayTimer;
		
		private bool _isWaitingForJump;

		private const float TERMINAL_VELOCITY = 53.0f;

		public void TryJump(bool isJumpRequested)
		{
			if (_groundChecker.IsGrounded)
			{
				if (_verticalVelocity < 0.0f)
					_verticalVelocity = -2f;

				_jumpAnimLayer.IsGrounded(true);
				_jumpAnimLayer.IsFreeFall(false);

				if (isJumpRequested && _jumpTimeoutDelta <= 0.0f)
				{
					if (!_isWaitingForJump)
					{
						_isWaitingForJump = true;
						_jumpDelayTimer = _jumpDelay;
						_jumpAnimLayer.PlayJumpAnim();
					}
				}

				if (_jumpTimeoutDelta >= 0.0f)
					_jumpTimeoutDelta -= Time.deltaTime;	
			}
			else
			{
				_jumpAnimLayer.IsGrounded(false);
				_jumpAnimLayer.IsFreeFall(true);
			}
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

			_jumpAnimLayer.IsGrounded(false);
			_jumpAnimLayer.IsFreeFall(true);
		}


		public void CheckAndRestVelocity()
		{
			if (_verticalVelocity < 0)
				_verticalVelocity = -2f;
		}

		public void ApplyGravity()
		{
			if (_verticalVelocity < TERMINAL_VELOCITY)
				_verticalVelocity += _gravity * Time.deltaTime;
		}

		public Vector3 GetJumpVector() => new (0.0f, _verticalVelocity, 0.0f);

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
			else if (_groundChecker.IsGrounded)
			{
				_jumpAnimLayer.StopJumpAnim();
			}
		}

	}
}