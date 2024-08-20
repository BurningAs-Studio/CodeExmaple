using UnityEngine;
using PetWorld.UI;
using Zenject;
using System;

namespace PetWorld.Player
{
	[Serializable]
	public class PlayerAim : IReadOnlyPlayerAim
	{
		[SerializeField] private UnInteractableScreen _aimScreen;
		[SerializeField] private PlayerCrosshair _crosshair;
		[SerializeField] private LayerMask _targetLayers;
		[SerializeField] private Transform _aimTarget;

		[Inject] [NonSerialized] private PlayerAnimationRig _animationRig;
		[Inject] [NonSerialized] private IWeaponHolder _weaponHolder;
		[Inject] [NonSerialized] private Camera _mainCamera;

		private Vector3 _screenCenter;
		
		public DamageableCollider CurrentTarget {get; private set;}
		public Vector3 LastAimedPosition {get; private set;}
		
		public bool IsHasTargetInAim => CurrentTarget != null;
		public bool IsActive { get; private set;}
		
		public event Action OnDisable;
		public event Action OnEnable;

		public void Initialize()
		{
			_screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0);
		}
		
		public void Enable()
		{
			_animationRig.EnableTorsoRig();
			_aimScreen.Show();
			_crosshair.ResetSize();
			IsActive = true;
			OnEnable?.Invoke();
		}

		public void Disable()
		{
			IsActive = false;
			_animationRig.DisableTorsoRig();
			_aimScreen.Hide();
			OnDisable?.Invoke();
		}
		
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
			LastAimedPosition = GetAimTargetPosition(ray);
		}
		
		private Vector3 GetAimTargetPosition(Ray ray)
		{
			if (Physics.Raycast(ray, out var hit, _weaponHolder.Data.ShootingRange))
				return hit.point;
			
			return ray.GetPoint(_weaponHolder.Data.ShootingRange);
		}
		
		private DamageableCollider GetDamageableInAim(Ray ray)
		{
			DamageableCollider target = null;

			_crosshair.SetDefaultView();
			
			if (_weaponHolder.IsWeaponHolding
			    && Physics.Raycast(ray, out var hit, _weaponHolder.Data.ShootingRange, _targetLayers))
			{
				if (hit.transform.TryGetComponent(out DamageableCollider damageable))
				{
					target = damageable;
					_crosshair.SetAggressiveView();
				}
			}
			return target;
		}
	}
}