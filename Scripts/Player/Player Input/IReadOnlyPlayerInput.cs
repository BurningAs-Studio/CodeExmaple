using UnityEngine;

namespace PetWorld.Player
{
    public interface IReadOnlyPlayerInput
    {
        public bool IsMovementJoystickActive { get;}
        public bool IsButtonsInputActive { get;}
        
        public Vector2 GetJoystickDirection();
        public Vector3 GetJoystickDirectionNormalized();
    }
}