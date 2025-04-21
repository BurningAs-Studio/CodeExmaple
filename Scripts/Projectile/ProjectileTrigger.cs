using UnityEngine;
using System;

namespace FAS.Projectiles
{
	public class ProjectileTrigger : MonoBehaviour
	{
		public event Action<ITarget> OnTriggerTarget;
		
		private void OnTriggerEnter(Collider other)
		{
			if (other.TryGetComponent(out ITarget target))
				OnTriggerTarget?.Invoke(target);
		}
	}
}