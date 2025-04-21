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

		protected virtual void OnEnable()
		{
			Health.OnZeroHealth += Destroy;
		}

		protected virtual void OnDisable()
		{
			Health.OnZeroHealth -= Destroy;
		}

		protected abstract void Destroy();
	}
}
