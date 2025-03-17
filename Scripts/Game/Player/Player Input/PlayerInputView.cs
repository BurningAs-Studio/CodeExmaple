using UnityEngine;
using System;
using FAS.UI;

namespace FAS.Players
{
	[Serializable]
	public class PlayerInputView
	{
		[field: SerializeField] public CustomButton InteractButton;
		[field: SerializeField] public CustomButton AttackButton;
		[field: SerializeField] public CustomButton JumpButton;
		[field: SerializeField] public CustomButton ChangeWeaponButton;
		[field: SerializeField] public CustomButton KickButton;
		[field: SerializeField] public CustomButton PunchButton;
		[field: SerializeField] public CustomButton SpeedBoostButton;
	}
}