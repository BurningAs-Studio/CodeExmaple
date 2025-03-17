using FAS.Players.Animations;
using UnityEngine;
using Zenject;

namespace FAS.Allies
{
	public class Ally : MonoBehaviour
	{
		[Inject] private PlayerAnimator _animator;
		[Inject] private AllyMover _mover;

		private Vector3 _lastPlayerPosition;

		private void Start() => _animator.SetGroundedState(true);

		//private void Update() => _mover.MoveTo(_target.transform.position);
	}
}