using PetWorld.Pets;
using Cinemachine;
using PetWorld.UI;
using UnityEngine;
using VInspector;
using Zenject;

namespace PetWorld.Player
{
    public class PlayerInstaller : MonoInstaller
    {
        [Foldout("Dependencies")]
        [SerializeField] private Player _player;
        [Space(10)]
        [SerializeField] private PlayerAnimatorEventReceiver _animatorEventReceiver;
        [SerializeField] private PlayerPetBallInteraction _petBallInteraction;
        [SerializeField] private CharacterController _characterController;
        [SerializeField] private PlayerAnimationRig _animationRigging;
        [SerializeField] private PlayerInteractables _interactables;
        [SerializeField] private PlayerCameraRotator _cameraRotator;
        [SerializeField] private PlayerStatesMachine _statesMachine;
        [SerializeField] private PlayerSoundEffects _soundEffects;
        [SerializeField] private CamerasSwitcher _camerasSwitcher;
        [SerializeField] private UIScreenSwitcher _screenSwitcher;
        [SerializeField] private DamageReceiver _damageReceiver;
        [SerializeField] private GroundChecker _groundChecker;
        [SerializeField] private PlayerAnimator _animator;
        [SerializeField] private Equipment _equipment;
        [SerializeField] private CraftMenu _craftMenu;
        [SerializeField] private PlayerHealth _health;
        [SerializeField] private Inventory _inventory;
        [SerializeField] private Camera _mainCamera;

        public override void InstallBindings()
        {
            BindFromInventoryPopup();
            BindFromEquipment();
            BindFromInventory();
            BindFromInstance();
            BindFromAnimator();
            BindFromPlayer();
            Bind();
        }

        private void BindFromInventoryPopup()
        {
            Container.BindInstance(_inventory.Popups.Resource).AsSingle();
        }
        
        private void BindFromEquipment()
        {
            Container.BindInstance(_equipment.Slots).AsSingle();
        }
        
        private void BindFromInventory()
        {
            Container.BindInstance(_inventory.Resources);
            Container.BindInstance(_inventory.Ammo);
        }

        private void BindFromInstance()
        {
            Container.Bind<IReadOnlyCamerasSwitcher>().FromInstance(_camerasSwitcher).AsSingle();
            Container.Bind<IReadOnlyPlayerAim>().FromInstance(_player.Attacker.Aim).AsSingle();
            Container.Bind<IReadOnlyStateMachine>().FromInstance(_statesMachine).AsSingle();
            Container.Bind<IPlayerAnimatorRestarter>().FromInstance(_animator).AsSingle();
            Container.Bind<IReadOnlyPlayerInput>().FromInstance(_player.Input).AsSingle();
            Container.Bind<IPlayerStatesHolder>().FromInstance(_statesMachine).AsSingle();
            Container.Bind<IPlayerInputControl>().FromInstance(_player.Input).AsSingle();
            Container.Bind<IReadOnlyPlayerJump>().FromInstance(_player.Jump).AsSingle();
            Container.Bind<IInventoryAccess>().FromInstance(_inventory).AsSingle();
            Container.Bind<ISpawnable>().FromInstance(_player).AsSingle();
            Container.Bind<Transform>().FromInstance(transform);
        }
        
        private void BindFromAnimator()
        {
            Container.BindInstance(_animator.TakeDamageLayer);
            Container.BindInstance(_animator.ShootingLayer);
            Container.BindInstance(_animator.ReloadLayer);
            Container.BindInstance(_animator.BaseLayer);
            Container.BindInstance(_animator.DeadLayer);
        }

        private void BindFromPlayer()
        {
            Container.BindInstance(_player.Attacker.WeaponVisibility).AsSingle();
            Container.BindInstance(_player.Attacker.WeaponHolder).AsSingle();
            Container.BindInstance(_player.WatterChecker).AsSingle();
            Container.BindInstance(_player.VisualEffects).AsSingle();
            Container.BindInstance(_player.Attacker.Aim).AsSingle();
            Container.BindInstance(_player.CameraShaker).AsSingle();
            Container.BindInstance(_player.Input.Events).AsSingle();
            Container.BindInstance(_player.Input.View).AsSingle();
            Container.BindInstance(_player.Building).AsSingle();
            Container.BindInstance(_player.Attacker).AsSingle();
            Container.BindInstance(_player.Rotator).AsSingle();
            Container.BindInstance(_player.Mover).AsSingle();
            Container.BindInstance(_player.Jump).AsSingle();
        }

        private void Bind()
        {
            Container.BindInstance(_animatorEventReceiver).AsSingle();
            Container.BindInstance(_characterController).AsSingle();
            Container.BindInstance(_petBallInteraction).AsSingle();
            Container.BindInstance(_animationRigging).AsSingle();
            Container.BindInstance(_camerasSwitcher).AsSingle();
            Container.BindInstance(_damageReceiver).AsSingle();
            Container.BindInstance(_screenSwitcher).AsSingle();
            Container.BindInstance(_groundChecker).AsSingle();
            Container.BindInstance(_cameraRotator).AsSingle();
            Container.BindInstance(_interactables).AsSingle();
            Container.BindInstance(_statesMachine).AsSingle();
            Container.BindInstance(_soundEffects).AsSingle();
            Container.BindInstance(_mainCamera).AsSingle();
            Container.BindInstance(_craftMenu).AsSingle();
            Container.BindInstance(_equipment).AsSingle();
            Container.BindInstance(_health).AsSingle();
        }

#if UNITY_EDITOR
        [Button("FIND DEPENDENCIES")]
        public void FindAllDependencies()
        {
            _mainCamera = GetComponentInChildren<CinemachineBrain>(true).GetComponent<Camera>();
            _animatorEventReceiver = GetComponentInChildren<PlayerAnimatorEventReceiver>(true);
            _petBallInteraction = GetComponentInChildren<PlayerPetBallInteraction>(true);
            _characterController = GetComponentInChildren<CharacterController>(true);
            _animationRigging = GetComponentInChildren<PlayerAnimationRig>(true);
            _interactables = GetComponentInChildren<PlayerInteractables>(true);
            _cameraRotator = GetComponentInChildren<PlayerCameraRotator>(true);
            _statesMachine = GetComponentInChildren<PlayerStatesMachine>(true);
            _soundEffects = GetComponentInChildren<PlayerSoundEffects>(true);
            _camerasSwitcher = GetComponentInChildren<CamerasSwitcher>(true);
            _screenSwitcher = GetComponentInChildren<UIScreenSwitcher>(true);
            _damageReceiver = GetComponentInChildren<DamageReceiver>(true);
            _groundChecker = GetComponentInChildren<GroundChecker>(true);
            _animator = GetComponentInChildren<PlayerAnimator>(true);
            _equipment = GetComponentInChildren<Equipment>(true);
            _craftMenu = GetComponentInChildren<CraftMenu>(true);
            _health = GetComponentInChildren<PlayerHealth>(true);
            _inventory = GetComponentInChildren<Inventory>(true);
            _player = GetComponent<Player>();
        }
#endif
    }
}