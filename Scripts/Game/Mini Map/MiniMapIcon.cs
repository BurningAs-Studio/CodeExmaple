using UnityEngine;

namespace FAS.MiniMap
{
	public class MiniMapIcon : MonoBehaviour
	{
		private const float DEFAULT_POSITION_Y = 112;

		private void Update()
		{
			var targetPosition = transform.position;
			targetPosition.y = DEFAULT_POSITION_Y;
			transform.position = targetPosition;
		}
	}
}