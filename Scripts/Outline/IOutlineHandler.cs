using UnityEngine;

namespace FAS
{
	public interface IOutlineHandler
	{
		public void ProcessOutline(Vector3 origin, Vector3 targetPosition);
	}
}