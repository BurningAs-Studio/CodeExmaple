using UnityEngine;

namespace FAS
{
	[RequireComponent(typeof(DamageReceiver))]
	[RequireComponent(typeof(Health))]
	public abstract class Destroyable : MonoBehaviour
	{
		protected DamageReceiver DamageReceiver;
		protected Health Health;

		protected virtual void Awake()
		{
			DamageReceiver = GetComponent<DamageReceiver>();
			Health = GetComponent<Health>();
		}

		private void OnEnable()
		{
			Health.OnZeroHealth += Destroy;
		}

		private void OnDisable()
		{
			Health.OnZeroHealth -= Destroy;
		}

		protected abstract void Destroy();
	}
}
