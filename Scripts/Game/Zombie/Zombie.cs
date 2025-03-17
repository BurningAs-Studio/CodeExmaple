using UnityEngine;
using Zenject;

namespace FAS.Zombies
{
	public class Zombie : MonoBehaviour
	{
		[Inject] private ZombieStateMachine _stateMachine;
		[Inject] private ZombieAnimator _animator;
		[Inject] private Health _health;

		private void OnEnable()
		{
			_health.OnTakeDamage += OnTakeDamage;
		}

		private void OnDisable()
		{
			_health.OnTakeDamage -= OnTakeDamage;
		}

		private void Start()
		{
			_stateMachine.Initialize();
		}
		
		private void OnTakeDamage()
		{
			_animator.PlayTakeDamageAnim();
		}
	}
}