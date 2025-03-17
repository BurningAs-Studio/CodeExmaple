using UnityEngine.UI;
using UnityEngine;
using TMPro;

namespace FAS.UI
{
	public class ChangeWeaponButton : CustomButton, IWeaponView
	{
		[SerializeField] private Image _weaponImage;
		[SerializeField] private TextMeshProUGUI _ammoViewText;
		
		private const string MAX_AMMO_TEXT = "MAX";

		public void ChangeWeaponIcon(Sprite icon) => _weaponImage.sprite = icon;
		
		public void UpdateAmmoView(int ammo) => _ammoViewText.text = ammo.ToString();
		
		public void ShowMaxAmmoText() => _ammoViewText.text = MAX_AMMO_TEXT;
	}
}
