using UnityEngine;
using Zenject;

namespace PetWorld.Player
{
    public class PlayerStatesMachine : StateMachine, IPlayerStatesHolder
    {
        [SerializeField] private InteractionState _interactionState;
        [SerializeField] private SwimmingState _swimmingState;
        [SerializeField] private RespawnState _respawnState;
        [SerializeField] private DefaultState _defaultState;
        [SerializeField] private AimingState _aimingState;
        [SerializeField] private DeadState _deadState;
        
        public InteractionState InteractionState => _interactionState;
        public SwimmingState SwimmingState => _swimmingState;
        public RespawnState RespawnState => _respawnState;
        public DefaultState DefaultState => _defaultState;
        public AimingState AimingState => _aimingState;
        public DeadState DeadState => _deadState;
        
        [Inject]
        public void Construct(DiContainer diContainer)
        {
            diContainer.Inject(_interactionState);
            diContainer.Inject(_swimmingState);
            diContainer.Inject(_respawnState);
            diContainer.Inject(_defaultState);
            diContainer.Inject(_aimingState);
            diContainer.Inject(_deadState);
        }

        private void Start()
        {
            _deadState.Initialize();
        }

        public override bool IsDynamicState(State state)
        {
            return state != _deadState;
        }

        protected override void SwitchStateTo(State nextState)
        {
            LastState = CurrentState;
            base.SwitchStateTo(nextState);
        }
    }
}