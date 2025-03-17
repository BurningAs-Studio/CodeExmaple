using UnityEngine;

namespace FAS.Players.Animations
{
	public class Layer : IReadOnlyAnimatorLayer
	{
		protected readonly Animator Animator;
		protected readonly int Index;

		protected virtual float EnableSpeed { get; private set; } = 10f;
		protected virtual float DisableSpeed { get; private set; } = 10f;
		protected virtual float MaxLayerWeight { get; private set; } = 1;
		
		public float Weight { get; private set; }
		
		public bool IsTransition => Animator.IsInTransition(Index);
		public bool IsEnabled => Weight >= MaxLayerWeight;
		public bool IsDisabled => Weight <= 0;

		public Layer(Animator animator, int index)
		{
			Animator = animator;
			Index = index;

			Weight = Animator.GetLayerWeight(Index);
		}
		
		public float CurrentAnimNTime => Animator.GetCurrentAnimatorStateInfo(Index).normalizedTime;

		public bool IsActive =>
			Animator.GetCurrentAnimatorStateInfo(Index).shortNameHash != PlayerAnimator.EmptyAnimHash;

		public void EnableWeightSmooth() =>
			SetWeight(Mathf.MoveTowards(Weight, MaxLayerWeight, EnableSpeed * Time.deltaTime));
		
		public void DisableWeightSmooth() =>
			SetWeight(Mathf.MoveTowards(Weight, 0, DisableSpeed * Time.deltaTime));

		
		private void SetWeight(float weight)
		{
			Weight = weight;
			Animator.SetLayerWeight(Index, Weight);
		}
	}
}