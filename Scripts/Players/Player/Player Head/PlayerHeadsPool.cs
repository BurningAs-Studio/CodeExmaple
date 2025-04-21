using System.Collections.Generic;
using UnityEngine;

namespace FAS.Players
{
	public class PlayerHeadsPool : MonoBehaviour
	{
		[SerializeField] private List<PickupHead> _headPrefabs;

		private readonly Dictionary<PlayerSkinType, Queue<PickupHead>> _headPools = new();
		private readonly Dictionary<PlayerSkinType, PickupHead> _prefabMap = new();

		private void Awake()
		{
			foreach (var prefab in _headPrefabs)
			{
				if (prefab == null) continue;

				var type = prefab.HeadType;
				_prefabMap.TryAdd(type, prefab);

				if (!_headPools.ContainsKey(type))
					_headPools[type] = new Queue<PickupHead>();
			}

			var existingHeads = GetComponentsInChildren<PickupHead>(includeInactive: true);
			foreach (var head in existingHeads)
			{
				var type = head.HeadType;

				if (!_headPools.ContainsKey(type))
					_headPools[type] = new Queue<PickupHead>();

				head.Initialize();
				head.OnReadyToRespawn += ReturnToPool;
				head.gameObject.SetActive(false);

				_headPools[type].Enqueue(head);
			}
		}

		public PickupHead Get(PlayerSkinType type)
		{
			if (!_headPools.TryGetValue(type, out var queue))
			{
				Debug.LogError($"No pool found for type: {type}");
				return null;
			}

			if (queue.Count > 0)
			{
				var reused = queue.Dequeue();
				reused.gameObject.SetActive(true);
				reused.transform.SetParent(null);
				return reused;
			}

			if (!_prefabMap.TryGetValue(type, out var prefab))
			{
				Debug.LogError($"No prefab assigned for PlayerSkinType: {type}");
				return null;
			}

			var newHead = Instantiate(prefab, transform);
			newHead.OnReadyToRespawn += ReturnToPool;
			newHead.gameObject.SetActive(true);
			return newHead;
		}

		private void ReturnToPool(PickupHead head)
		{
			head.OnReadyToRespawn -= ReturnToPool;
			head.Initialize();
			head.transform.localScale = Vector3.one;

			if (!_headPools.TryGetValue(head.HeadType, out var queue))
				queue = _headPools[head.HeadType] = new Queue<PickupHead>();

			queue.Enqueue(head);
			head.OnReadyToRespawn += ReturnToPool;
		}
	}
}
