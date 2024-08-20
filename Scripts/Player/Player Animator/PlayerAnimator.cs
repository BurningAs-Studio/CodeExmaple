using UnityEngine;
using Zenject;

namespace PetWorld.Player
{
	public class PlayerAnimator : MonoBehaviour, IPlayerAnimatorRestarter
	{
		[field: SerializeField] public BaseAnimatorLayer BaseLayer {get; private set;}
		[field: SerializeField] public ShootingAnimatorLayer ShootingLayer {get; private set;}
		[field: SerializeField] public ReloadAnimatorLayer ReloadLayer {get; private set;}
		[field: SerializeField] public TakeDamageAnimatorLayer TakeDamageLayer {get; private set;}
		[field: SerializeField] public DeadAnimatorLayer DeadLayer {get; private set;}
		
		[Inject(Id = PlayerAnimatorType.Main)] private Animator _animator;

		public static readonly int EmptyAnimHash = Animator.StringToHash("Empty");

		[Inject]
		public void Construct(DiContainer diContainer)
		{
			diContainer.Inject(TakeDamageLayer);
			diContainer.Inject(ShootingLayer);
			diContainer.Inject(ReloadLayer);
			diContainer.Inject(BaseLayer);
			diContainer.Inject(DeadLayer);
		}

		public void Restart()
		{
			_animator.Rebind();
		}

		private void Update()
		{
			ShootingLayer.UpdateStateInfo();
		}
                
		//PETBALL LOGIC - REWORK THIS!!!
		// public void EnablePetBallLayer()
		// {
		//     _petballLayerWeightTween.Kill();
		//     _petballLayerWeightTween = DOVirtual.Float(_animator.GetLayerWeight(PETBALL_LAYER), 1, _enableReloadLayerDuration, 
		//             weight => _animator.SetLayerWeight(PETBALL_LAYER, weight))
		//         .SetEase(Ease.Flash);
		// }
		//
		// public void DisablePetBallLayer()
		// {
		//     _petballLayerWeightTween.Kill();
		//     _petballLayerWeightTween = DOVirtual.Float(_animator.GetLayerWeight(PETBALL_LAYER), 0, _disableReloadLayerDuration, 
		//             weight => _animator.SetLayerWeight(PETBALL_LAYER, weight))
		//         .SetEase(Ease.Flash);
		// }
        
		// public void EnablePetballAnim()
		// {
		//     _animator.SetBool(IsPetballAim, true);
		// }
		//
		// public void DisablePetballAnim()
		// {
		//     _animator.SetBool(IsPetballAim, false);
		// }
		//
		// public void PlayThrowPetBallAnim()
		// {
		//     _animator.SetTrigger(IdleThrowPetballTrigger);
		// }
		//
		// public void PlayAimThrowPetBallAnim()
		// {
		//     _animator.Play(ThrowAimPetballAnimHash, PETBALL_LAYER, 0);
		// }
		//private Tween _petballLayerWeightTween;
		//private const int PETBALL_LAYER = 4;
		// public readonly int ThrowAimPetballAnimHash = Animator.StringToHash("Throw Petball Aim");
		// public readonly int HoldingPetballAnimHash = Animator.StringToHash("Holding Petball");
		// private readonly int IdleThrowPetballTrigger = Animator.StringToHash("idleThrowPetball");
		// private readonly int IsPetballAim = Animator.StringToHash("isPetballAim");
	}
}