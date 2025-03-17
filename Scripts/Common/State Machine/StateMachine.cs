using UnityEngine;

namespace FAS
{
	public abstract class StateMachine : MonoBehaviour
	{
		private State _requestedState;

		public State CurrentState { get; protected set; }
		public State LastState { get; protected set; }

		public abstract void Initialize();

		public void RequestSwitchStateTo(State state)
		{
			_requestedState = state;
		}

		protected virtual void SwitchStateTo(State nextState)
		{
			LastState = CurrentState;
			
			if (nextState != CurrentState)
			{
				CurrentState?.Exit();
				CurrentState = nextState;
			}
			
			CurrentState?.Enter();
		}

		protected virtual void Update()
		{
			CurrentState?.Perform();
		}

		private void LateUpdate()
		{
			if (_requestedState != null)
			{
				SwitchStateTo(_requestedState);
				_requestedState = null;
			}
			else if (CurrentState != null && CurrentState.IsReadyToTransit)
			{
				SwitchStateTo(CurrentState.NextState);
			}
		}
	}
}