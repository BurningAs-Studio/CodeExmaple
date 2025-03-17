using UnityEngine;

namespace FAS
{
	public interface IWeaponView
	{
		public void ChangeWeaponIcon(Sprite icon);
		
		public void UpdateAmmoView(int ammo);

		public void ShowMaxAmmoText();
	}
}