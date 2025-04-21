using FAS.Players.States;
using Zenject;

namespace FAS.Players
{
	public class PlayerStateMachine : StateMachine
	{
		[Inject] private Unarmed _unarmedState;

		public override void Initialize()
		{
			SwitchStateTo(_unarmedState);
		}
	}
}