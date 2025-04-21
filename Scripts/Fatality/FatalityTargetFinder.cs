using System.Collections.Generic;
using UnityEngine;
using System;

namespace FAS.Fatality
{
	public class FatalityTargetFinder : MonoBehaviour, IReadOnlyFatalityTargetFinder
	{
		private readonly HashSet<IFatalityTarget> _targets = new ();
		
		public bool IsHasTarget => _targets.Count > 0;

		public event Action OnFindFirstTarget;
		public event Action OnLoseAllTarget;

		private void OnDestroy()
		{
			if (_targets.Count > 0)
			{
				foreach (var target in _targets)
				{
					target.OnNotReadyToFatality -= RemoveTarget;
					target.OnReadyToFatality -= AddTarget;
				}
			}
		}

		private void OnTriggerEnter(Collider other)
		{
			if (other.TryGetComponent(out IFatalityTarget target))
			{
				target.OnNotReadyToFatality += RemoveTarget;
				target.OnReadyToFatality += AddTarget;
				
				if (target.IsReadyToFatality)
					AddTarget(target);
			}
		}

		private void OnTriggerExit(Collider other)
		{
			if (other.TryGetComponent(out IFatalityTarget target))
			{
				target.OnNotReadyToFatality -= RemoveTarget;
				target.OnReadyToFatality -= AddTarget;
				RemoveTarget(target);
			}
		}

		private void RemoveTarget(IFatalityTarget target)
		{
			_targets.Remove(target);
			
			if (!IsHasTarget)
				OnLoseAllTarget?.Invoke();
		}

		private void AddTarget(IFatalityTarget target)
		{
			if (!IsHasTarget)
				OnFindFirstTarget?.Invoke();

			_targets.Add(target);
		}

		public bool IsHasReadyToFatalityTarget(out IFatalityTarget fatalityTarget)
		{
			fatalityTarget = null;
			var closestDistanceSqr = float.MaxValue;
			var currentPos = transform.position;

			foreach (var target in _targets)
			{
				var distanceSqr = (target.Position - currentPos).sqrMagnitude;

				if (distanceSqr < closestDistanceSqr)
				{
					closestDistanceSqr = distanceSqr;
					fatalityTarget = target;
				}
			}	

			return fatalityTarget != null;
		}
	}
}