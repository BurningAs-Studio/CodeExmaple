namespace PetWorld.Player
{
    public interface IPlayerStatesHolder
    {
        public InteractionState InteractionState {get;}
        public SwimmingState SwimmingState {get;}
        public DefaultState DefaultState {get;}
        public RespawnState RespawnState {get;}
        public AimingState AimingState {get;}
        public DeadState DeadState {get;}
    }
}