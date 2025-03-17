using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VInspector;

namespace FAS
{
    public class RespawnPointsHolder : MonoBehaviour
    {
        [SerializeField] private Transform _playerTransform;
        [SerializeField] private Transform _zombiesPointsHolder;
        [SerializeField] private List<RespawnPoint> _zombiesPoints = new();
        [SerializeField] private float _checkPlayerRadius = 20f;

        [SerializeField] private RespawnPoint[] _nearbyZombiesPoints;
        private float _nextTimeCheckPlayerPosition;

        private const int MAX_NEARBY_ZOMBIES_POINTS = 3;
        private const float CHECK_PLAYER_DELAY = 1f;

        private void Awake()
        {
            if (_playerTransform == null)
                Debug.LogError("Player Transform is not assigned!");
        }

        private void Start()
        {
            _nearbyZombiesPoints = new RespawnPoint[MAX_NEARBY_ZOMBIES_POINTS];
            foreach (var point in _zombiesPoints)
                point.SetRadius(_checkPlayerRadius);

            CheckPoints();
        }

        public Vector3 GetEnemyPosition()
        {
	        return _nearbyZombiesPoints[Random.Range(0, _nearbyZombiesPoints.Length)].transform.position;
        }

        private void CheckPoints()
        {
	        var playerPos = _playerTransform.position;

	        var best = new RespawnPoint[MAX_NEARBY_ZOMBIES_POINTS];
	        float[] bestDist = { float.MaxValue, float.MaxValue, float.MaxValue };

	        for (int i = 0; i < _zombiesPoints.Count; i++)
	        {
		        var point = _zombiesPoints[i];

		        if (point.IsHasTarget)
		        {
			        point.SqrDistanceToPlayer = float.MaxValue;
		        }
		        else
		        {
			        point.SqrDistanceToPlayer = Vector3.SqrMagnitude(point.transform.position - playerPos);

			        float dist = point.SqrDistanceToPlayer;

			        for (int j = 0; j < bestDist.Length; j++)
			        {
				        if (dist < bestDist[j])
				        {
					        for (int k = bestDist.Length - 1; k > j; k--)
					        {
						        bestDist[k] = bestDist[k - 1];
						        best[k] = best[k - 1];
					        }

					        bestDist[j] = dist;
					        best[j] = point;
					        break;
				        }
			        }
		        }
	        }

	        _nearbyZombiesPoints = best.Where(p => p != null).ToArray();
	        _nextTimeCheckPlayerPosition = Time.timeSinceLevelLoad + CHECK_PLAYER_DELAY;
        }

        private void Update()
        {
            if (Time.timeSinceLevelLoad > _nextTimeCheckPlayerPosition)
                CheckPoints();
        }

#if UNITY_EDITOR
        private bool _isEnemiesPositionsShown;

        [Button]
        private void FindPoints()
        {
	        _zombiesPoints.Clear();
	        _zombiesPoints.AddRange(_zombiesPointsHolder.GetComponentsInChildren<RespawnPoint>());
        }

        [Button]
        private void ShowEnemiesGizmos() => _isEnemiesPositionsShown = true;

        [Button]
        private void HideEnemiesGizmos() => _isEnemiesPositionsShown = false;

        private void OnDrawGizmos()
        {
            if (_isEnemiesPositionsShown && _zombiesPoints.Count > 0)
            {
                Gizmos.color = ColorExtensions.Orange;
                foreach (var point in _zombiesPoints)
                {
                    Gizmos.DrawCube(point.transform.position + Vector3.up, Vector3.one + Vector3.up);
                    Gizmos.DrawWireSphere(point.transform.position, _checkPlayerRadius);
                }
            }
        }
#endif
    }
}