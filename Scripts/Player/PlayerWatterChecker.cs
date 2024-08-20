using UnityEngine;
using Zenject;
using System;

namespace PetWorld.Player
{
	[Serializable]
	public class PlayerWatterChecker
	{
		[SerializeField] private float _checkWaterDistance = 2f;
		
		[Inject] private Transform _transform;

		public bool IsWatter()
		{
			var ray = new Ray(_transform.position, Vector3.down);
			return Physics.Raycast(ray, out var hit, _checkWaterDistance) && hit.transform.TryGetComponent(out Water water);
		}
	}
}