using UnityEngine;

namespace PetWorld.Player
{
    public interface IReadOnlyPlayerJump
    {
        public Vector3 GetJumpVector();
    }
}