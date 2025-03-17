using System.Collections.Generic;
using FAS.Projectiles;
using Cinemachine;
using UnityEngine;
using Zenject;

namespace FAS
{
	[RequireComponent(typeof(WeaponAnimator))]
	public class Weapon : MonoBehaviour, IWeaponData
	{
		[field: SerializeField] public WeaponType Type { get; private set; }
		[field: SerializeField] public CinemachineImpulseSource ShotCameraImpulse { get; private set; }
		[field: SerializeField] public float FocusShotFadeStep { get; private set; } = 0.1f;
		[field: SerializeField] public int CrosshairShotSizeStep { get; private set; } = 25;
		[field: SerializeField] public float ShootingRange { get; private set; } = 10;
		[field: SerializeField] public int Damage { get; private set; } = 1;
		[field: SerializeField] public float EnableLeftHandSpeed { get; private set; } = 0.1f;
		[field: SerializeField] public Transform LeftHandPoint { get; private set; }
		[field: SerializeField] public Sprite Icon { get; private set; }

		[Range(0, 999)][SerializeField] private int _magazineSize = 30;
		[Range(0, 999)][SerializeField] private int _startAmmo = 30;
		[SerializeField] private float _shootingDelay;
		[SerializeField] private List<AudioClip> _shootAudioClips;
		[SerializeField] private ParticleSystem _muzzleFlash;
		[SerializeField] private Transform _bulletSpawnPoint;

		[Inject] private ProjectilePool _projectilePool;
		[Inject] private IWeaponView _view;
		
		private WeaponAnimator _animator;
		
		private float _timeToNextShot;

		private bool _isEquipped;

		public IEnumerable<AudioClip> ShootAudioClips => _shootAudioClips;

		public bool IsCanShoot => _timeToNextShot < Time.timeSinceLevelLoad;
		
		public int AmmoInMagazine {get; private set;}

		private void Awake()
		{
			_animator = GetComponent<WeaponAnimator>();
			
			AmmoInMagazine = _startAmmo;
		}

		public void Equip()
		{
			_view.ChangeWeaponIcon(Icon);
			
			if (AmmoInMagazine < _magazineSize)
				_view.UpdateAmmoView(AmmoInMagazine);
			else
				_view.ShowMaxAmmoText();

			_isEquipped = true;
		}

		public void Unequip()
		{
			_isEquipped = false;
		}
		
		public void AddAmmo(int amount)
		{
			AmmoInMagazine = Mathf.Min(AmmoInMagazine + amount, _magazineSize);

			if (_isEquipped)
			{
				if (AmmoInMagazine >= _magazineSize)
					_view.ShowMaxAmmoText();
				else
					_view.UpdateAmmoView(AmmoInMagazine);
			}
		}

		public void SetEmpty() => _animator.PlayEmptyAnim();

		public void Fire(Vector3 targetPosition, DamageReceiver damageReceiver, bool isHasObstacle,
			DamageableCollider targetCollider)
		{
			AmmoInMagazine--;
			_view.UpdateAmmoView(AmmoInMagazine);
			_animator.PlayFireAnim();
			_muzzleFlash.Play();
			_timeToNextShot = _shootingDelay + Time.timeSinceLevelLoad;
			var bullet = _projectilePool.Get();
			bullet.Launch(targetPosition, _bulletSpawnPoint.position, Damage, damageReceiver, isHasObstacle,
				targetCollider);
		}
	}
}