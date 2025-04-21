using System.Collections.Generic;
using FAS.Fatality;
using UnityEngine;
using Zenject;

namespace FAS.Players.States
{
	public class ExecuteFatality : SkillCastState
	{
		[SerializeField] private List<FatalityData> _data = new ();
		
		[Inject] private FatalityTargetFinder _targetFinder;
		[Inject] private IFatalityPopup _popup;
		
		private IFatalityTarget _currentTarget;

		private int _currentDataIndex;

		private bool _isWaitingToStartAnimation;
		
		public override void Enter()
		{
			base.Enter();
			_isWaitingToStartAnimation = true;
			
			if (_targetFinder.IsHasTarget && _targetFinder.IsHasReadyToFatalityTarget(out var target))
			{
				var currentData = _data[_currentDataIndex];
				_currentTarget = target;
				AnimEvents.OnFatality += OnFatality;
				_currentTarget.StartFatality(currentData);
				CamerasSwitcher.EnableFatalityCamera();
				UIScreensSwitcher.ShowFatalityScreen();
				Animator.SetLocomotionValue(0);
				Animator.PlayExecuteFatalityAnim(currentData.Type);
				currentData.StartFatalityEffect.Play();
			}
			else
			{
				RequestTransition(UnarmedState);
			}
		}

		private void OnFatality()
		{
			CameraShaker.PlayFatalityTornadoKickImpulse();
			_currentTarget.PerformFatality();
		}

		public override void Perform()
		{
			base.Perform();
			Jump.ApplyGravity();

			if (_isWaitingToStartAnimation)
			{
				if (Animator.FatalityLayer.IsActive)
					_isWaitingToStartAnimation = false;
			}
			else if (!Animator.FatalityLayer.IsActive)
				RequestTransition(UnarmedState);
		}
		
		public override void Exit()
		{
			if (_currentTarget != null)
			{
				AnimEvents.OnFatality -= OnFatality;
				UIScreensSwitcher.ShowDefaultScreen();
				CamerasSwitcher.DisableFatalityCamera();
				_currentTarget.FinishFatality();
				_popup.Show();
			}
			_currentTarget = null;
			base.Exit();
		}
	}
}