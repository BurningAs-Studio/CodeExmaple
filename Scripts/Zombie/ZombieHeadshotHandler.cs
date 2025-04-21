using UnityEngine;
using Zenject;

namespace FAS.Zombies
{
	public class ZombieHeadshotHandler : HeadshotHandler
	{
		[Inject] private ZombieAnimationRig _animationRig;
		[Inject] private IHeadshotView _view;

		protected override void OnHeadshot()
		{
			_animationRig.RequestEnableHead();
			base.OnHeadshot();
			_animationRig.SetHeadTargetRotation(TargetRotation.eulerAngles);
			_view.ShowView();
		}

		protected override void RotateHead(Quaternion currentRotation)
		{
			_animationRig.RequestEnableHead();
			base.RotateHead(currentRotation);
			_animationRig.SetHeadTargetRotation(TargetRotation.eulerAngles);
		}

		protected override Quaternion GetHeadTargetRotation() => _animationRig.GetHeadTargetRotation();
	}
}