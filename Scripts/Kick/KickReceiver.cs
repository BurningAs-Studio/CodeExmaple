using System.Collections;
using UnityEngine;
using Zenject;
using System;

namespace FAS
{
	public class KickReceiver : MonoBehaviour, IKickable
	{
		[Inject] private PuppetMasterHandler _puppetMasterHandler;
		[Inject] private Health _health;
		
		private Coroutine _currentKickCoroutine;
		private Rigidbody _hipsRigidbody;
		
		public Vector3 Position => transform.position;
		
		public bool IsReadyToKick { get; private set; } = true;

		public event Action OnKick;

		private void Awake()
		{
			_hipsRigidbody = _puppetMasterHandler.GetMuscle(0).joint.GetComponent<Rigidbody>();
		}
		
		public void SetReadyToKick()
		{
			IsReadyToKick = true;
		}

		public virtual void DeathKick(Vector3 kickPosition, Vector3 directionOffset, float force = 1000)
		{
			_health.TryTakeDamage(float.MaxValue);
			Kick(kickPosition, directionOffset, force);
		}

		public void Kick(Vector3 kickPosition, Vector3 directionOffset, float force)
		{
			if (_currentKickCoroutine != null)
				StopCoroutine(_currentKickCoroutine);
			
			_currentKickCoroutine = StartCoroutine(KickRoutine(kickPosition, directionOffset, force));
		}

		private IEnumerator KickRoutine(Vector3 kickPosition, Vector3 directionOffset, float force)
		{
			IsReadyToKick = false;
			OnKick?.Invoke();
			yield return null;
			var direction = (transform.position - kickPosition).normalized;
			_hipsRigidbody.AddForce((direction + directionOffset) * force, ForceMode.Impulse);
		}
	}
}