using UnityEngine;
using System;

namespace PetWorld.Player
{
    [Serializable]
    public class PlayerJump : IReadOnlyPlayerJump
    {
        [SerializeField] private float _jumpHeight = 1.2f;
        [SerializeField] private float _gravity = -15.0f;
        [SerializeField] private float _jumpTimeout = 0.50f;
        [SerializeField] private float _fallTimeout = 0.15f;

        private float _verticalVelocity;
        private float _jumpTimeoutDelta;
        private float _fallTimeoutDelta;

        private readonly float _terminalVelocity = 53.0f;

        public void TryJump(bool isJumpRequested)
        {
            _fallTimeoutDelta = _fallTimeout;

            if (_verticalVelocity < 0.0f)
                _verticalVelocity = -2f;

            if (isJumpRequested && _jumpTimeoutDelta <= 0.0f)
                _verticalVelocity = Mathf.Sqrt(_jumpHeight * -2f * _gravity);

            if (_jumpTimeoutDelta >= 0.0f)
                _jumpTimeoutDelta -= Time.deltaTime;
        }

        public void FreeFall()
        {
            _jumpTimeoutDelta = _jumpTimeout;

            if (_fallTimeoutDelta >= 0.0f)
                _fallTimeoutDelta -= Time.deltaTime;
        }
        
        public void ApplyGravity()
        {
            if (_verticalVelocity < _terminalVelocity)
                _verticalVelocity += _gravity * Time.deltaTime;
        }
        
        public Vector3 GetJumpVector()
        {
            return new Vector3(0.0f, _verticalVelocity, 0.0f);
        }
    }
}