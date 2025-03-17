using FAS.Players.Animations;
using UnityEngine.AI;
using UnityEngine;
using Zenject;

namespace FAS.Allies
{
    public class AllyMover : MonoBehaviour
    {
        [SerializeField] private float _smoothingFactor = 10f;

        [Inject] private PlayerAnimator _animator;
        [Inject] private NavMeshAgent _agent;

        private float _currentSpeed;

        public void MoveTo(Vector3 targetPosition)
        {
            _agent.SetDestination(targetPosition);
            UpdateMoveAnim();
        }

        private void UpdateMoveAnim()
        {
            _currentSpeed = Mathf.Lerp(_currentSpeed, GetNormalizedSpeed(), Time.deltaTime * _smoothingFactor);
            _animator.SetLocomotionValue(_currentSpeed);
        }

        private float GetNormalizedSpeed()
        {
            return _agent.velocity.magnitude / _agent.speed;
        }

        private void Update()
        {
            UpdateMoveAnim();
        }
    }
}
