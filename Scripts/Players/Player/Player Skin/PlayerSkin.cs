using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VInspector;
using Zenject;
using System;

namespace FAS.Players
{
	public enum PlayerSkinType
	{
		Chicken = 0,
		Bear = 1,
		Buffalo = 2,
		Cat = 3,
		Dog = 4,
		Duck = 5,
		Elephant = 6,
		Frog = 7,
		Monkey = 8,
		Pig = 9,
		Rabbit = 10,
		Rhino = 11
	}
	
	public class PlayerSkin : MonoBehaviour
	{
		[SerializeField] private SkinnedMeshRenderer _bodyMeshRenderer;
		[SerializeField] private bool _isUseRandomSkin = true;
		[HideIf(nameof(_isUseRandomSkin))]
		[SerializeField] private PlayerSkinType _skinType = PlayerSkinType.Chicken;
		[EndIf]
		[SerializeField] private List<PlayerSkinData> _skinDatas = new ();

		[Inject] private PlayerHead _head;
		
		public PlayerSkinData Data { get; private set; }

		private void Awake()
		{
			PlayerSkinType skinType;

			if (_isUseRandomSkin)
			{
				var values = Enum.GetValues(typeof(PlayerSkinType));
				skinType = (PlayerSkinType)values.GetValue(UnityEngine.Random.Range(0, values.Length));
			}
			else
			{
				skinType = _skinType;
			}

			Data = _skinDatas.First(skinData => skinData.Type == skinType);
			var materials = _bodyMeshRenderer.materials;
			materials[0] = Data.BodyMaterial;
			_bodyMeshRenderer.materials = materials;
			_head.Instantiate(skinType, Data.HeadPrefab);
		}
	}
}