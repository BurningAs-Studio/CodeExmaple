using UnityEngine;

namespace FAS
{
	public class GroundChecker : MonoBehaviour
	{
		[SerializeField] private float _groundedOffset = 0.13f;
		[SerializeField] private float _groundedRadius = 1f;
		[SerializeField] private LayerMask _groundLayers;
        
		public bool IsGrounded { get; private set; }

		public void CheckGround()
		{
			var spherePosition = new Vector3(transform.position.x, transform.position.y - _groundedOffset,
				transform.position.z);

			IsGrounded = Physics.CheckSphere(spherePosition, _groundedRadius, _groundLayers,
				QueryTriggerInteraction.Ignore);
		}
        
#if UNITY_EDITOR
		private void OnDrawGizmos()
		{
			Gizmos.color = IsGrounded ? Color.green : Color.red;
			var spherePosition = new Vector3(transform.position.x, transform.position.y - _groundedOffset,
				transform.position.z);
			Gizmos.DrawWireSphere(spherePosition, _groundedRadius);
		}
#endif
	}
}