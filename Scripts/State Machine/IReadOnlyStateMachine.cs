namespace PetWorld
{
    public interface IReadOnlyStateMachine
    {
        public bool IsDynamicState(State state);

        public State CurrentState {get;}
        
        public State LastState { get;}
    }
}