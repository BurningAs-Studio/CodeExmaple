using FAS.Pickups;
using UnityEngine;
using System;

namespace FAS.Players
{
	[RequireComponent(typeof(BoxCollider))]
	[RequireComponent(typeof(Rigidbody))]
	public class PickupHead : Pickupable
	{
		[field: SerializeField] public PlayerSkinType HeadType {get; private set;}
		
		[SerializeField] private int _dynamicLayer;
		[SerializeField] private ParticleSystem _bloodEffect;

		private BoxCollider _collider;
		private Rigidbody _rigidbody;
		private Vector3 _lastPosition;

		private bool _isStaticState;
		
		private int _staticChecksCount;
		private int _defaultLayerIndex;

		private float _nextTimePositionCheck;
		private float _nextTimeRespawn;

		private const int STATIC_THRESHOLD = 4;
		
		private const float STATIC_THRESHOLD_CHECK_INTERVAL = 0.25f;
		private const float THROW_FROM_EXPLOSION_HEIGHT_OFFSET = 50f;
		private const float THROW_FROM_FATALITY_HEIGHT_OFFSET = 20f;
		private const float RESPAWN_DELAY = 5f;
		private const float THROW_FORCE = 20f;
		
		public event Action<PickupHead> OnReadyToRespawn;

		private void Awake()
		{
			_collider = GetComponent<BoxCollider>();
			_rigidbody = GetComponent<Rigidbody>();

			_defaultLayerIndex = gameObject.layer;
		}

		public void Initialize()
		{
			Respawn();
			gameObject.SetActive(false);
		}
		
		public override void Apply(IPickupVisitor visitor) => visitor.Visit(this);

		public void ThrowFromFatality()
			=> Throw(((-transform.forward + transform.right) * THROW_FORCE)
			         + (Vector3.up * THROW_FROM_FATALITY_HEIGHT_OFFSET));

		public void ThrowFromExplosion()
			=> Throw(Vector3.up * THROW_FROM_EXPLOSION_HEIGHT_OFFSET);

		private void Throw(Vector3 direction)
		{
			_isStaticState = false;
			_collider.enabled = true;
			_rigidbody.isKinematic = false;
			_rigidbody.AddForce(direction, ForceMode.Impulse);
			gameObject.layer = _dynamicLayer;
			_bloodEffect.Play();
		}

		public override void Pickup()
		{
			base.Pickup();
			DisablePhysics();
			_bloodEffect.Pause();
			_nextTimeRespawn = Time.timeSinceLevelLoad + RESPAWN_DELAY;
		}

		private void DisablePhysics()
		{
			_collider.enabled = false;
			_rigidbody.isKinematic = true;
			gameObject.layer = _defaultLayerIndex;
		}

		private void ReadyToRespawn()
		{
			DisablePhysics();
			_nextTimeRespawn = float.MaxValue;
			OnReadyToRespawn?.Invoke(this);
		}

		private void SetStaticState()
		{
			DisablePhysics();
			_bloodEffect.Pause();
			_isStaticState = true;
		}

		protected override void Update()
		{
			if (IsPickedUp)
			{
				if (Time.timeSinceLevelLoad > _nextTimeRespawn)
					ReadyToRespawn();
			}
			else if (!_isStaticState)
			{
				if (Time.timeSinceLevelLoad > _nextTimePositionCheck)
				{
					_nextTimePositionCheck = Time.timeSinceLevelLoad + STATIC_THRESHOLD_CHECK_INTERVAL;

					if (transform.position == _lastPosition)
					{
						_staticChecksCount++;

						if (_staticChecksCount >= STATIC_THRESHOLD)
							SetStaticState();
					}
					else
					{
						_staticChecksCount = 0;
						_lastPosition = transform.position;
					}
				}
			}
		}
	}
}