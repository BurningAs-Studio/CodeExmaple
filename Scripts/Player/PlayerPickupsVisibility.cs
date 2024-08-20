using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PetWorld
{
	[RequireComponent(typeof(Collider))]
	public class PlayerPickupsVisibility : MonoBehaviour
	{
		private readonly HashSet<PickupHolder> _visibleHolders = new HashSet<PickupHolder>();
		
		private float _nextCheckTime;
		
		private const float CHECK_COOLDOWN = 1f;

		private void OnTriggerEnter(Collider other)
		{
			if (other.TryGetComponent(out PickupHolder holder))
			{
				if (holder.IsHolding)
					holder.ShowPickup();
				else if (holder.IsReadyToHold())
					holder.SpawnPickup();
				
				_visibleHolders.Add(holder);
			}
		}

		private void OnTriggerExit(Collider other)
		{
			if (other.TryGetComponent(out PickupHolder holder))
			{
				if (holder.IsHolding)
					holder.HidePickup();
				
				_visibleHolders.Remove(holder);
			}
		}
		
		private void CheckVisibleHolders()
		{
			foreach (var holder in _visibleHolders.Where(holder => !holder.IsHolding && holder.IsReadyToHold()))
				holder.SpawnPickup();
		}

		private void Update()
		{
			if (Time.timeSinceLevelLoad > _nextCheckTime)
			{
				CheckVisibleHolders();
				_nextCheckTime = Time.timeSinceLevelLoad + CHECK_COOLDOWN;
			}
		}
	}
}