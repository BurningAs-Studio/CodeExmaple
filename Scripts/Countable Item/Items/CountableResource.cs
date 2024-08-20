using UnityEngine;
using System;

namespace PetWorld
{
	[Serializable]
	public class CountableResource : CountableItem
	{
		[SerializeField] private Resource _resource;
		
		public ResourceType Type => _resource.Type;
		
		public override int Amount
		{
			get => _resource.Amount;
			protected set => _resource.Amount = value;
		}
	}
}