using UnityEngine.UI;
using UnityEngine;
using System;

namespace FAS.LootBoxes
{
	[Serializable]
	public struct LootBoxItemData
	{
		public ItemType Type;
		public string Name;
		public string Description;
		public Sprite SpinSprite;
		public Sprite DropSprite;
		public float DropChance;
	}
	
	public class LootBoxItem : MonoBehaviour
	{
		[SerializeField] private Image _image;

		public LootBoxItemData Data { get; private set; }

		public void Initialize(LootBoxItemData data)
		{
			_image.sprite = data.SpinSprite;
			Data = data;
		}

		public void SetPosition(Vector3 position)
		{
			transform.position = position;
		}
	}
}