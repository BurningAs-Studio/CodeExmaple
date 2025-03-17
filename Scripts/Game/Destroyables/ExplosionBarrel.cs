using System.Collections.Generic;
using UnityEngine;

namespace FAS
{
    public class ExplosionBarrel : Destroyable
    {
        [SerializeField] private float _explosionRadius = 5f;
        [SerializeField] private int _explosionDamage = 100;
        [SerializeField] private LayerMask _damageableLayers;
        [SerializeField] private int _maxColliders = 10;
        [SerializeField] private MeshRenderer _body;
        [SerializeField] private ParticleSystem _explosionEffect;
        [SerializeField] private float _respawnTime = 5f;
        [SerializeField] private LayerMask _obstacleLayers;
        [SerializeField] private Transform _obstacleCheckingPoint;
        [SerializeField] private CapsuleCollider _collider;

        private Collider[] _colliderBuffer;
        private float _timeToRespawn;
        private bool _isDestroyed;

#if UNITY_EDITOR
        private readonly List<(Vector3 start, Vector3 end, Color color)> _debugLines = new();
#endif

        protected override void Awake()
        {
            base.Awake();
            _colliderBuffer = new Collider[_maxColliders];
        }

        protected override void Destroy()
        {
            _body.enabled = false;
            _collider.enabled = false;
            _explosionEffect.Play();
            Explode();
        }

        private void Explode()
        {
            DamageReceiver.DisableDamageableColliders();
            Health.enabled = false;

#if UNITY_EDITOR
            _debugLines.Clear();
#endif

            int hits = Physics.OverlapSphereNonAlloc(
                transform.position, _explosionRadius, _colliderBuffer, _damageableLayers);

            for (int i = 0; i < hits; i++)
            {
                if (_colliderBuffer[i].TryGetComponent(out DamageableCollider damageable))
                {
                    var targetPosition = _colliderBuffer[i].transform.position;
                    targetPosition.y = _obstacleCheckingPoint.position.y;

                    var isBlocked = 
	                    Physics.Linecast(_obstacleCheckingPoint.position, targetPosition, _obstacleLayers);

                    if (!isBlocked)
	                    damageable.TakeDamage(_explosionDamage, DamageReceiver);
#if UNITY_EDITOR
                    _debugLines.Add((
	                    _obstacleCheckingPoint.position, targetPosition, 
	                    isBlocked ? Color.red : Color.green));
#endif
                }
            }

            _timeToRespawn = Time.timeSinceLevelLoad + _respawnTime;
            _isDestroyed = true;
        }

        private void Respawn()
        {
	        _collider.enabled = true;
            DamageReceiver.EnableDamageableColliders();
            Health.enabled = true;
            Health.TryHeal(Health.StartHealth);
            _body.enabled = true;
            _isDestroyed = false;
        }

        private void Update()
        {
            if (_isDestroyed && Time.timeSinceLevelLoad >= _timeToRespawn)
                Respawn();

#if UNITY_EDITOR
            DrawDebugLines();
#endif
        }

#if UNITY_EDITOR
        private void DrawDebugLines()
        {
            foreach (var line in _debugLines)
            {
                Debug.DrawLine(line.start, line.end, line.color);
            }
        }

        private void OnDrawGizmos()
        {
            if (!Application.isPlaying)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(transform.position, _explosionRadius);
            }
        }
#endif
    }
}
