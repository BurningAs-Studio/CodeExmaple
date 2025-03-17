using FAS.Players.States;
using Zenject;

namespace FAS.Players
{
	public class PlayerStateMachine : StateMachine
	{
		[Inject] private Idle _idleState;

		public override void Initialize()
		{
			SwitchStateTo(_idleState);
		}
	}
}