namespace PetWorld
{
    public abstract class State : ListenerSubscriber
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
        }
		
        public abstract void Enter();
        public abstract void Perform();
    }
}