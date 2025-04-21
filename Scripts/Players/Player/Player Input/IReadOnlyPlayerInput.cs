using UnityEngine;

namespace FAS.Players
{
	public interface IReadOnlyPlayerInput
	{
		public bool IsMovementJoystickActive { get;}
		public bool IsButtonsInputActive { get;}
        
		public Vector2 GetJoystickDirection2D();
		
		public Vector3 GetJoystickDirection3D();
	}
}