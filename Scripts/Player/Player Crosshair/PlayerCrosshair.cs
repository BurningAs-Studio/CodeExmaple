using UnityEngine;
using System;

namespace PetWorld.Player
{
    [Serializable]
    public class PlayerCrosshair
    {
        [SerializeField] private PlayerCrosshairView _view;
        [SerializeField] private Vector3 _defaultScale;
        [SerializeField] private Vector3 _aggressiveScale;
        [Range(MIN_SIZE, MAX_SIZE)][SerializeField] private float _defaultSize;
        [Range(MIN_SIZE, MAX_SIZE)][SerializeField] private float _movementSize;
        [SerializeField] private float _moveToDefaultSizeSpeed = 5f;
        [SerializeField] private float _moveToMovementSizeSpeed = 5f;
        
        private const float MAX_SIZE = 500f;
        private const float MIN_SIZE = 200f;
        
        private float _currentSize;

        public void MoveToMovementSize() => SmoothChangeSize(_movementSize, _moveToMovementSizeSpeed);

        public void MoveToDefaultSize() => SmoothChangeSize(_defaultSize, _moveToDefaultSizeSpeed);

        public void SetAggressiveView()
        {
            _view.ChangeColorToRed();
            _view.SetScale(_aggressiveScale);
        }

        public void SetDefaultView()
        {
            _view.ChangeColorToWhite();
            _view.SetScale(_defaultScale);
        }
        
        public void ResetSize()
        {
            _currentSize = _defaultSize;
            _view.SetSize(_currentSize);
        }

        public void AddSize(float stepSize)
        {
            var targetSize = _currentSize + stepSize;
            
            if (targetSize > MAX_SIZE)
                targetSize = MAX_SIZE;
            
            ChangeSize(targetSize);
        }

        private void ChangeSize(float size)
        {
            _currentSize = size;
            _view.SetSize(_currentSize);
        }
        
        private void SmoothChangeSize(float size, float speed)
        {
            _currentSize = Mathf.MoveTowards(_currentSize, size, speed * Time.deltaTime);
            _view.SetSize(_currentSize);
        }
    }   
}