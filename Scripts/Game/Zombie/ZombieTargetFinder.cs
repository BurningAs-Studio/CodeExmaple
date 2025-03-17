using UnityEngine;

namespace FAS.Zombies
{
	public class ZombieTargetFinder : MonoBehaviour
	{
		[SerializeField] private float _loseTargetDelay = 3f;
		[SerializeField] private bool _isNeededEyesContact;
        [Tooltip("Obstacle Layers - For Line Cast Only")]
        [SerializeField] private LayerMask _eyesContactLayers;

        private float _nextEyeContactCheckTime;
        private float _timeToLoseTarget;

        private bool _loseTargetRequested;
        
        private const float CHECK_EYE_CONTACT_INTERVAL = 1f;
        
        public ITarget CurrentTarget {get; private set;}
        
        public bool IsHasTarget {get; private set;}
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out ITarget target))
            {
                CurrentTarget = target;

                if (_isNeededEyesContact)
                {
                    IsHasTarget = IsHasEyesContact();
                    _nextEyeContactCheckTime = Time.timeSinceLevelLoad + CHECK_EYE_CONTACT_INTERVAL;
                }
                else
                {
                    IsHasTarget = true;
                }
                
                _loseTargetRequested = false;
            }
        }

        private void OnTriggerExit(Collider other)
        {
	        if (other.TryGetComponent(out ITarget target) && CurrentTarget == target)
	        {
	            _timeToLoseTarget = _loseTargetDelay + Time.timeSinceLevelLoad;
		        _loseTargetRequested = true;
	        }
        }

        public Vector3 GetCurrentTargetPosition()
        {
	        return CurrentTarget.Position;
        }

        public void LoseTarget()
        {
	        _loseTargetRequested = false;
            CurrentTarget = null;
            IsHasTarget = false;
        }

        private bool IsHasEyesContact()
        {
            return !Physics.Linecast(transform.position + Vector3.up,
	            CurrentTarget.Position + Vector3.up, _eyesContactLayers, QueryTriggerInteraction.Ignore);
        }

        private void Update()
        {
            // if (_isNeededEyesContact && !IsHasTarget && Time.timeSinceLevelLoad > _nextEyeContactCheckTime)
            // {
            //     IsHasTarget = IsHasEyesContact();
            //     _nextEyeContactCheckTime = Time.timeSinceLevelLoad + CHECK_EYE_CONTACT_INTERVAL;
            // }
            if (_loseTargetRequested && Time.timeSinceLevelLoad > _timeToLoseTarget)
            {
	            LoseTarget();
            }
        }
        
#if UNITY_EDITOR
        [Header("Debug")]
        [SerializeField] private bool _showGizmos;

        private void OnDrawGizmos()
        {
            if (_showGizmos && CurrentTarget != null)
            {
                Gizmos.color = IsHasEyesContact() ? Color.green : Color.red;
                Gizmos.DrawLine(transform.position + Vector3.up, CurrentTarget.Position + Vector3.up);
            }
        }
#endif
	}
}