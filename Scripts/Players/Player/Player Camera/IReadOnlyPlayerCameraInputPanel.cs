using UnityEngine;

namespace FAS.Players
{
	public interface IReadOnlyPlayerCameraInputPanel
	{
		public Vector2 CurrentInputVector { get; }
        
		public bool IsInputProcess { get; }
	}
}