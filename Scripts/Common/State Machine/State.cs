using UnityEngine;

namespace FAS
{
	public abstract class State : MonoBehaviour
	{
		public State NextState {get; protected set;}
		
		public bool IsReadyToTransit {get; protected set;}

		public virtual void Exit()
		{
			IsReadyToTransit = false;
		}

		protected void RequestTransition(State nextState)
		{
			NextState = nextState;
			IsReadyToTransit = true;
			return;
		}
		
		public abstract void Enter();
		public abstract void Perform();
	}
}