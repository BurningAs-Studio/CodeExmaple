using UnityEngine;

namespace FAS
{
	public interface IKickable
	{
		public Vector3 Position { get;}
		
		public bool IsReadyToKick { get; }
		
		public void DeathKick(Vector3 kickPosition, Vector3 directionOffset, float force = 500);

		public void Kick(Vector3 kickPosition, Vector3 directionOffset, float force = 1000);
	}
}