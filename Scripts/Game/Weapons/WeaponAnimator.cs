using UnityEngine;

namespace FAS
{
	[RequireComponent(typeof(Animator))]
	public class WeaponAnimator : MonoBehaviour
	{
		private Animator _animator;
		
		private readonly int _isEmptyBoolAnimHash = Animator.StringToHash("isEmpty");
		private readonly int _fireAnimHash = Animator.StringToHash("Fire");

		private void Awake() => _animator = GetComponent<Animator>();

		public void PlayFireAnim() => _animator.Play(_fireAnimHash, 0, 0);
		
		public void PlayEmptyAnim() => _animator.SetBool(_isEmptyBoolAnimHash, true);
		
		public void StopEmptyAnim() => _animator.SetBool(_isEmptyBoolAnimHash, false);
	}
}