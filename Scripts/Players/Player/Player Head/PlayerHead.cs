using UnityEngine;
using Zenject;

namespace FAS.Players
{
	public class PlayerHead : MonoBehaviour
	{
		[Inject] private PlayerHeadsPool _headsPool;
		[Inject] private DiContainer _diContainer;
		
		private GameObject _body;
		
		private readonly Vector3 _pickupHeadScale = new (0.01f, 0.01f, 0.01f);
		
		public PlayerSkinType HeadType {get; private set;}
		
		public void Instantiate(PlayerSkinType headType, GameObject body)
		{
			_body = _diContainer.InstantiatePrefab(body, transform);
			HeadType = headType;
		}

		public void ExplosionTakeOff() => TakeOff().ThrowFromExplosion();

		public void FatalityTakeOff() => TakeOff().ThrowFromFatality();

		private PickupHead TakeOff()
		{
			_body.SetActive(false);
			var pickupHead = _headsPool.Get(HeadType);
			pickupHead.transform.localScale = _pickupHeadScale;
			pickupHead.transform.SetPositionAndRotation(transform.position, transform.rotation);
			return pickupHead;
		}

		public void TakeOn()
		{
			_body.SetActive(true);
		}
	}
}