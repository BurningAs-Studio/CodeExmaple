using PetWorld.Player.Building;
using PetWorld.Player.Pets;
using PetWorld.Pets;
using UnityEngine;
using Zenject;

namespace PetWorld.Player
{
	[RequireComponent(typeof(PlayerPetBallInteraction))]
	[RequireComponent(typeof(PlayerCameraRotator))]
	[RequireComponent(typeof(CharacterController))]
	[RequireComponent(typeof(PlayerStatesMachine))]
	[RequireComponent(typeof(PlayerAnimationRig))]
	[RequireComponent(typeof(PlayerPetSpawner))]
	[RequireComponent(typeof(DamageReceiver))]
	[RequireComponent(typeof(GroundChecker))]
	[RequireComponent(typeof(PlayerHealth))]
	public class Player : MonoBehaviour, ISpawnable
	{
		[field: SerializeField] public PlayerVisualEffects VisualEffects;
		[field: SerializeField] public PlayerWatterChecker WatterChecker;
		[field: SerializeField] public PlayerCameraShaker CameraShaker;
		[field: SerializeField] public PlayerAttacker Attacker;
		[field: SerializeField] public PlayerBuilding Building;
		[field: SerializeField] public PlayerRotator Rotator;
		[field: SerializeField] public PlayerInput Input;
		[field: SerializeField] public PlayerMover Mover;
		[field: SerializeField] public PlayerJump Jump;

		[Inject] private TakeDamageAnimatorLayer _takeDamageAnimator;
		[Inject] private IReadOnlyPlayerInputEvents _inputEvents;
		[Inject] private IInventoryAccess _inventory;
		[Inject] private PlayerHealth _health;
		
		private PlayerStatesMachine _statesMachine;
		
		public Vector3 SpawnPosition {get; private set;}
		
		[Inject]
		public void Constructor(DiContainer diContainer)
		{
			diContainer.Inject(WatterChecker);
			diContainer.Inject(_inventory);
			diContainer.Inject(Attacker);
			diContainer.Inject(Building);
			diContainer.Inject(Rotator);
			diContainer.Inject(Input);
			diContainer.Inject(Mover);
			diContainer.Inject(Jump);
		}

		private void Awake()
		{
			_statesMachine = GetComponent<PlayerStatesMachine>();
		}

		private void OnEnable()
		{
			_inputEvents.OnDisableAimModButtonClicked += DisableAimingState;
			_inputEvents.OnEnableAimModButtonClicked += EnableAimingState;
			_inputEvents.OnInteractButtonClicked += StartInteraction;

			_health.OnTakeDamage += OnTakeDamage;
			_health.OnTakeHeal += OnHeal;
		}

		private void OnDisable()
		{
			_inputEvents.OnDisableAimModButtonClicked += DisableAimingState;
			_inputEvents.OnEnableAimModButtonClicked += EnableAimingState;
			_inputEvents.OnInteractButtonClicked -= StartInteraction;

			_health.OnTakeDamage -= OnTakeDamage;
			_health.OnTakeHeal -= OnHeal;
		}

		private void Start()
		{
			Input.Initialize();
			VisualEffects.Initialize();
			Attacker.Initialize();
			_health.Initialize();
			
			SpawnPosition = transform.position;
			_statesMachine.RequestSwitchStateTo(_statesMachine.DefaultState);
		}
		
		private void EnableAimingState()
		{
			_statesMachine.RequestSwitchStateTo(_statesMachine.AimingState);
		}
		
		private void DisableAimingState()
		{
			_statesMachine.RequestSwitchStateTo(_statesMachine.DefaultState);
		}
		
		private void StartInteraction()
		{
			_statesMachine.RequestSwitchStateTo(_statesMachine.InteractionState);
		}

		private void OnTakeDamage()
		{
			_takeDamageAnimator.PlayTakeDamageAnim();
			CameraShaker.PlayTakeDamageImpulse();
			VisualEffects.PlayTakeDamageEffect();
		}

		private void OnHeal()
		{
			VisualEffects.PlayHealEffect();
		}
	}
}
