using FAS.Players.AnimRig;
using UnityEngine;
using Zenject;

namespace FAS.Players
{
	public class PlayerHeadshotHandler : HeadshotHandler
	{
		[Inject] private PlayerAnimationRig _animationRig;

		protected override void OnHeadshot()
		{
			_animationRig.RequestEnableHead();
			base.OnHeadshot();
			_animationRig.SetHeadTargetRotation(TargetRotation.eulerAngles);
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