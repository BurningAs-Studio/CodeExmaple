using System.Collections.Generic;
using UnityEngine;

namespace FAS
{
    public class ExplosionBarrel : Destroyable
    {
        [SerializeField] private Vector3 _explosionKickOffset = new (0f, 0.5f, 0f);
        [SerializeField] private int _explosionDamage = 100;
        [SerializeField] private MeshRenderer _body;
        [SerializeField] private ParticleSystem _explosionEffect;
        [SerializeField] private float _respawnTime = 5f;
        [SerializeField] private CapsuleCollider _collider;
        [SerializeField] private KickTrigger _trigger;

        private float _timeToRespawn;
        private bool _isDestroyed;

#if UNITY_EDITOR
        private readonly List<(Vector3 start, Vector3 end, Color color)> _debugLines = new();
#endif
        
        protected override void OnEnable()
        {
	        base.OnEnable();
	        _trigger.OnTriggerKickable += PhysicalKick;
	        _trigger.OnTriggerDamageable += TakeDamage;
        }

        protected override void OnDisable()
        {
	        base.OnDisable();
	        _trigger.OnTriggerKickable -= PhysicalKick;
	        _trigger.OnTriggerDamageable -= TakeDamage;
        }
        
        private void PhysicalKick(IKickable kickable)
	        => kickable.DeathKick(transform.position, _explosionKickOffset);
		
        private void TakeDamage(DamageableCollider damageable)
	        => damageable.TakeDamage(float.MaxValue, DamageReceiver);

        protected override void Destroy()
        {
            _collider.enabled = false;
            _body.enabled = false;
            _explosionEffect.Play();
            Explode();
        }

        private void Explode()
        {
            DamageReceiver.DisableDamageableColliders();
            Health.enabled = false;
            _trigger.Enable();
            _timeToRespawn = Time.timeSinceLevelLoad + _respawnTime;
            _isDestroyed = true;
        }

        private void Respawn()
        {
	        _trigger.Disable();
	        _collider.enabled = true;
            DamageReceiver.EnableDamageableColliders();
            Health.enabled = true;
            Health.TryHeal(Health.StartHealth);
            _body.enabled = true;
            _isDestroyed = false;
        }

        private void Update()
        {
            if (_isDestroyed && Time.timeSinceLevelLoad > _timeToRespawn)
                Respawn();
#if UNITY_EDITOR
            DrawDebugLines();
#endif
        }

#if UNITY_EDITOR
        private void DrawDebugLines()
        {
            foreach (var line in _debugLines)
	            Debug.DrawLine(line.start, line.end, line.color);
        }
#endif
    }
}
