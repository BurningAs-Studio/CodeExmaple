using UnityEngine;

namespace FAS.Players.Animations
{
	public class FatalityLayer : Layer
	{
		private readonly int _fatalityExecuteAnimHash = Animator.StringToHash("fatalityExecute");
		private readonly int _fatalityReceiveAnimHash = Animator.StringToHash("fatalityReceive");
		private readonly int _fatalityTypeValueAnimHash = Animator.StringToHash("fatalityType");
		
		public FatalityLayer(Animator animator, int index) : base(animator, index)
		{
		}
		
		public void PlayExecuteFatalityAnim(int animType)
		{
			Animator.SetInteger(_fatalityTypeValueAnimHash, animType);
			Animator.SetTrigger(_fatalityExecuteAnimHash);
		}

		public void PlayReceiveFatalityAnim(int animType)
		{
			Animator.SetInteger(_fatalityTypeValueAnimHash, animType);
			Animator.SetTrigger(_fatalityReceiveAnimHash);
		}
	}
}