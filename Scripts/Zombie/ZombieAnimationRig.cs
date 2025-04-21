using UnityEngine.Animations.Rigging;
using UnityEngine;

namespace FAS.Zombies
{
	public class ZombieAnimationRig : MonoBehaviour
	{
		[Header("Head Rig")]
		[SerializeField] private Rig _head;
		[SerializeField] private Transform _headTarget;
		
		private readonly AnimRigWeightData _headWeightData = new ();
		
		public void SetHeadTargetRotation(Vector3 rotation) => _headTarget.localRotation = Quaternion.Euler(rotation);
		
		public Quaternion GetHeadTargetRotation() => _headTarget.localRotation;

		public void RequestEnableHead() => _headWeightData.RequestEnable();

		private void Update()
		{
			if (_headWeightData.IsWeightUpdated(out var headWeight))
				_head.weight = headWeight;
		}
	}
}