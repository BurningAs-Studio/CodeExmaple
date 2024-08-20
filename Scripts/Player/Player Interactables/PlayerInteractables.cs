using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using ModestTree;
using Zenject;
using System;

namespace PetWorld.Player
{
    [RequireComponent(typeof(CapsuleCollider))]
    public class PlayerInteractables : MonoBehaviour
    {
        [Inject] [NonSerialized] private IReadOnlyPlayerInputEvents _inputEvents;
        [Inject] [NonSerialized] private PlayerInputView _inputView;
        
        private readonly HashSet<Interactable> _interactables = new HashSet<Interactable>();
        private CapsuleCollider _collider;
        
        private void Awake()
        {
            _collider = GetComponent<CapsuleCollider>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Interactable interactable))
                Add(interactable);
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out Interactable interactable))
                Remove(interactable);
        }

        public void Add(Interactable interactable)
        {
            _inputView.EnableInteractButton();
            _interactables.Add(interactable);
        }
        
        public void Remove(Interactable interactable)
        {
            _interactables.Remove(interactable);
                
            if (_interactables.IsEmpty())
                _inputView.DisableInteractButton();
        }

        public Interactable GetClosestInteractable()
        {
            var closestInteractable = _interactables.First();

            if (_interactables.Count > 1)
            {
                var closestSqrMagnitude = float.MaxValue;
                var playerPosition = transform.position;

                foreach (var interactable in _interactables)
                {
                    var sqrMagnitude = Vector3.SqrMagnitude(playerPosition - interactable.transform.position);
                    
                    if (sqrMagnitude < closestSqrMagnitude)
                    {
                        closestSqrMagnitude = sqrMagnitude;
                        closestInteractable = interactable;
                    }
                }   
            }
            return closestInteractable;
        }
        
        public void Enable()
        {
            _collider.enabled = true;
        }

        public void Disable()
        {
            _collider.enabled = false;
        }
    }
}