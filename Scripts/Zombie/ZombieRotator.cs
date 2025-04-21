using UnityEngine.AI;
using UnityEngine;
using Zenject;

namespace FAS.Zombies
{
	public class ZombieRotator : MonoBehaviour
	{
		[Inject] private NavMeshAgent _agent;

		public void SetAutoAngularSpeed(float speed) => _agent.angularSpeed = speed;
	}
}