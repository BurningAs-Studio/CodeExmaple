using UnityEngine;
using Zenject;

namespace FAS.Players
{
    public class PlayerAim : MonoBehaviour, IReadOnlyPlayerAim
    {
        [SerializeField] private PlayerCrosshair _crosshair;
        [SerializeField] private LayerMask _targetLayers;

        [Inject] private IReadOnlyPlayerWeapon _weapon;
        [Inject] private DamageReceiver _damageReceiver;
        [Inject] private Camera _mainCamera;

        private Vector3 _screenCenter;

        public DamageableCollider CurrentTarget { get; private set; }
        public Vector3 LastAimedPosition { get; private set; }

        public bool IsHasTargetInAim => CurrentTarget != null;
        public bool IsHasObstacle {get; private set;}
        public bool IsActive { get; private set; } = true;

        private void Awake()
        {
            _screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        }

        public void HideCrosshair() => _crosshair.HideView();
        
        public void ShowCrosshair() => _crosshair.ShowView();

        public void AddCrosshairSize(float size)
        {
            _crosshair.AddSize(size);
        }

        public void UpdateCrosshairSize(bool isMovementProcess)
        {
            if (isMovementProcess)
                _crosshair.MoveToMovementSize();
            else
                _crosshair.MoveToDefaultSize();
        }

        public void CheckAimHits()
        {
            var ray = _mainCamera.ScreenPointToRay(_screenCenter);
            CurrentTarget = GetDamageableInAim(ray);
        }

        private DamageableCollider GetDamageableInAim(Ray ray)
        {
            DamageableCollider target = null;

            _crosshair.SetDefaultView();

            if (Physics.Raycast(ray, out var hit, _weapon.Data.ShootingRange, _targetLayers))
            {
	            if (hit.transform.TryGetComponent(out DamageableCollider damageable))
	            {
		            target = damageable;
		            _crosshair.SetAggressiveView();   
	            }

	            LastAimedPosition = hit.point;
	            IsHasObstacle = true;
            }
            else
            {
	            LastAimedPosition =  ray.GetPoint(_weapon.Data.ShootingRange);
	            IsHasObstacle = false;
            }
            
            return target;
        }
    }
}
