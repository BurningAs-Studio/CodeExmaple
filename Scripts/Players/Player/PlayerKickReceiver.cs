using UnityEngine;
using Zenject;

namespace FAS.Players
{
	public class PlayerKickReceiver : KickReceiver
	{
		[Inject] private PlayerHead _head;

		public override void DeathKick(Vector3 kickPosition, Vector3 directionOffset, float force = 1000)
		{
			base.DeathKick(kickPosition, directionOffset, force);
			_head.ExplosionTakeOff();
		}
	}
}