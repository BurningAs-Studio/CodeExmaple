using PetWorld.Player.Building;
using UnityEngine;
using System;

namespace PetWorld.Player
{
    [Serializable]
    public class PlayerInputView
    {
        [SerializeField] private GameObject _changeWeaponButtonsHolder;
        [SerializeField] private GameObject _aimButtonsHolder;

        [Header("Highlights")]
        [SerializeField] private GameObject _workbenchHighlight;

        private GameObject _closeInventoryButton;
        private GameObject _openInventoryButton;
        private GameObject _throwPetBallButton;
        private GameObject _mainWeaponButton;
        private GameObject _disableAimButton;
        private GameObject _enableAimButton;
        private GameObject _workbenchButton;
        private GameObject _interactButton;
        private GameObject _reloadButton;
        private GameObject _attackButton;
        private GameObject _pistolButton;

        public void Initialize(CustomButton closeInventoryButton, CustomButton openInventoryButton,
            CustomButton throwPetBallButton, CustomButton mainWeaponButton, CustomButton disableAimButton,
            CustomButton enableAimButton, WorkbenchButton workbenchButton, CustomButton interactButton,
            CustomButton reloadButton, CustomButton attackButton, CustomButton pistolButton)
        {
            _closeInventoryButton = closeInventoryButton.gameObject;
            _openInventoryButton = openInventoryButton.gameObject;
            _throwPetBallButton = throwPetBallButton.gameObject;
            _mainWeaponButton = mainWeaponButton.gameObject;
            _disableAimButton = disableAimButton.gameObject;
            _enableAimButton = enableAimButton.gameObject;
            _workbenchButton = workbenchButton.gameObject;
            _interactButton = interactButton.gameObject;
            _reloadButton = reloadButton.gameObject;
            _attackButton = attackButton.gameObject;
            _pistolButton = pistolButton.gameObject;
        }

        public void EnableWorkbenchButton()
        {
            _workbenchButton.SetActive(true);
        }

        public void DisableWorkbenchButton()
        {
            _workbenchButton.SetActive(false);
        }

        public void EnableMainWeaponButton()
        {
            EnableWeaponModeButtons();
            _mainWeaponButton.SetActive(true);
        }

        public void EnablePistolButton()
        {
            EnableWeaponModeButtons();
            _pistolButton.SetActive(true);
        }

        public void EnableWeaponModeButtons()
        {
            _aimButtonsHolder.SetActive(true);
            _attackButton.SetActive(true);
            EnableChangeWeaponButtons();
            EnableReloadButton();
        }

        public void DisableWeaponModeButtons()
        {
            _aimButtonsHolder.SetActive(false);
            _attackButton.SetActive(false);
            DisableChangeWeaponButtons();
            DisableReloadButton();
        }

        public void SwitchAimButtonToEnable()
        {
            _disableAimButton.SetActive(false);
            _enableAimButton.SetActive(true);
        }

        public void SwitchAimButtonToDisable()
        {
            _disableAimButton.SetActive(true);
            _enableAimButton.SetActive(false);
        }

        public void DisablePetballThrowButton()
        {
            _throwPetBallButton.SetActive(false);
        }

        public void EnablePetBallThrowButton()
        {
            _throwPetBallButton.SetActive(true);
        }

        public void HighlightWorkbenchButton()
        {
            _workbenchHighlight.SetActive(true);
        }

        public void DisableWorkbenchButtonHighlight()
        {
            _workbenchHighlight.SetActive(false);
        }

        public void EnableInteractButton()
        {
            _interactButton.SetActive(true);
        }

        public void DisableInteractButton()
        {
            _interactButton.SetActive(false);
        }

        public void DisableChangeWeaponButtons()
        {
            _changeWeaponButtonsHolder.SetActive(false);
        }

        public void EnableChangeWeaponButtons()
        {
            _changeWeaponButtonsHolder.SetActive(true);
        }

        public void DisableOpenInventoryButton()
        {
            _openInventoryButton.SetActive(false);
        }

        public void EnableOpenInventoryButton()
        {
            _openInventoryButton.SetActive(true);
        }

        public void DisableCloseInventoryButton()
        {
            _closeInventoryButton.SetActive(false);
        }

        public void EnableCloseInventoryButton()
        {
            _closeInventoryButton.SetActive(true);
        }

        public void DisableReloadButton()
        {
            _reloadButton.SetActive(false);
        }

        public void EnableReloadButton()
        {
            _reloadButton.SetActive(true);
        }
    }
}
