using FAS.FakePlayers.States;
using VInspector;
using Zenject;

namespace FAS.FakePlayers
{
	public class FakePlayerStateMachine : StateMachine
	{
		[Inject] private Puppet _puppetState;
		[Inject] private Idle _idleState;

		public override void Initialize()
		{
			SwitchStateTo(_idleState);
		}
		
		protected override void SwitchStateTo(State nextState)
		{
			base.SwitchStateTo(nextState);
			print($"FakePlayer State {CurrentState}");
		}

		[Button]
		private void SwitchStateToPuppet() => SwitchStateTo(_puppetState);
	}
}