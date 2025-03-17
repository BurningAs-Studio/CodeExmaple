using FAS.Zombies.States;
using Zenject;

namespace FAS.Zombies
{
	public class ZombieStateMachine : StateMachine
	{
		[Inject] private Idle _idleState;
		
		public override void Initialize()
		{
			SwitchStateTo(_idleState);
		}
	}
}