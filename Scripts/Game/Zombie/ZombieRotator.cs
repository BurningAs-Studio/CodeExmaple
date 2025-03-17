using UnityEngine.AI;
using UnityEngine;
using Zenject;

namespace FAS.Zombies
{
	public class ZombieRotator : MonoBehaviour
	{
		[Inject] private ZombieTargetFinder _targetFinder;
		[Inject] private NavMeshAgent _agent;

		private bool _isDLookAtTargetRequested;

		public void RequestLookAtTarget()
		{
			_isDLookAtTargetRequested = true;
		}

		public void SetAutoAngularSpeed(float speed) => _agent.angularSpeed = speed;

		private void LateUpdate()
		{
			// if (_isDLookAtTargetRequested)
			// {
			// 	if (_targetFinder.IsTargetFound)
			// 	{
			// 		var target = _targetFinder.CurrentTarget.transform.position;
			// 		target.y = transform.position.y;
			// 		transform.LookAt(target);	
			// 	}
			// 	
			// 	_isDLookAtTargetRequested = false;
			// }
		}
	}
}