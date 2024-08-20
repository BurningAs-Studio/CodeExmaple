using UnityEngine;
using Zenject;
using System;

namespace PetWorld.Player
{
    [Serializable]
    public class PlayerWeaponChangerView
    {
        [SerializeField] private SelectWeaponButton _mainWeaponButton;
        [SerializeField] private SelectWeaponButton _pistolButton;
        
        [Inject] [NonSerialized] private PlayerInputView _inputView;
        
        private SelectWeaponButton[] _buttons;
        
        public void Initialize()
        {
            _buttons = new []
            {
                _mainWeaponButton,
                _pistolButton
            };
        }

        public void UpdateMainWeaponButtonAmmoCount(int ammo) =>  _mainWeaponButton.UpdateInventoryAmmoView(ammo);

        public void UpdatePistolButtonAmmoCount(int ammo) => _pistolButton.UpdateInventoryAmmoView(ammo);

        public void EnableMainWeaponButton() => _inputView.EnableMainWeaponButton();
        
        public void EnablePistolButton() => _inputView.EnablePistolButton();
        
        public void SelectMainWeaponButton(Sprite weaponImage) => ChangeSelectedButton(_mainWeaponButton, weaponImage);
        
        public void SelectPistolButton(Sprite weaponImage) => ChangeSelectedButton(_pistolButton, weaponImage);

        private void ChangeSelectedButton(SelectWeaponButton selectedButton, Sprite weaponImage)
        {
            foreach (var button in _buttons)
            {
                if (button == selectedButton)
                {
                    button.EnableHighlight();
                    button.SetWeaponImage(weaponImage);
                }
                else
                {
                    button.DisableHighlight();
                }
            }
        }
    }
}