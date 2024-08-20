using UnityEngine;
using Zenject;
using System;

namespace PetWorld.Player
{
    [Serializable]
    public class PlayerRotator
    {
        [SerializeField] private float _rotationSpeed = 5f;

        [Inject] [NonSerialized] private Transform _transform;
        
        public void RotateTo(Vector3 direction)
        {
            if (direction != Vector3.zero)
            {
                var targetRotation = Quaternion.LookRotation(direction, Vector3.up);
                _transform.rotation = Quaternion.Slerp(
                    _transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
            }
        }
        
        public void LookAt(Vector3 position)
        {
            var directionToTarget = _transform.position - position;
            var targetRotation = Quaternion.LookRotation(directionToTarget, Vector3.up);
            _transform.rotation = targetRotation;

            var fixedAngles = _transform.eulerAngles;
            fixedAngles.x = 0;
            fixedAngles.z = 0;
            _transform.eulerAngles = fixedAngles;
        }
    }
}