using FAS.Players.Animations;
using UnityEngine;
using Zenject;

namespace FAS.Players
{
	public class PlayerSpeedBoost : MonoBehaviour
	{
		[SerializeField] private float _speedMultiplier = 1.25f;
		[SerializeField] private float _duration = 2f;

		[Inject] private IReadOnlyPlayerInputEvents _inputEvents;
		[Inject] private PlayerVisualEffects _visualEffects;
		[Inject] private PlayerAnimator _animator;
		[Inject] private PlayerMover _mover;

		private float _disableBoostTime;

		private bool _isDisableRequested;

		private bool _isActive;

		private void OnEnable()
		{
			_inputEvents.OnSpeedBoostButtonClicked += Enable;
		}

		private void OnDisable()
		{
			_inputEvents.OnSpeedBoostButtonClicked -= Enable;
		}

		private void Enable()
		{
			_visualEffects.PlaySpeedBoostEffect();
			_disableBoostTime = Time.timeSinceLevelLoad + _duration;
			_isActive = true;
		}

		private void Disable()
		{
			_isActive = false;
		}

		private void BoostSpeed()
		{
			_animator.SetSpeedMultiplier(_speedMultiplier);
			_mover.SetSpeedMultiplier(_speedMultiplier);
		}

		public void RequestDisable() => _isDisableRequested = true;

		private void Update()
		{
			if (_isDisableRequested)
			{
				_isDisableRequested = false;
			}
			else if (_isActive)
			{
				if (Time.timeSinceLevelLoad < _disableBoostTime)
					BoostSpeed();
				else
					Disable();
			}
		}
	}
}