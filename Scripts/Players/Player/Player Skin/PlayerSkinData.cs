using UnityEngine;

namespace FAS.Players
{
	[CreateAssetMenu(fileName = "NewPlayerSkinData", menuName = "FAS/Skin Data", order = 0)]
	public class PlayerSkinData : ScriptableObject
	{
		[field: SerializeField] public PlayerSkinType Type { get; private set; }
		[field: SerializeField] public GameObject HeadPrefab { get; private set; }
		[field: SerializeField] public Material BodyMaterial { get; private set; }
	}
}