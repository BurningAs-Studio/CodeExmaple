using PetWorld.Player;
using UnityEngine;
using VInspector;
using Zenject;

public enum PlayerAnimatorType
{
    Main,
    Shooting
}

public class PlayerAnimatorInstaller : MonoInstaller
{
    [Foldout("Dependencies")]
    [SerializeField] private Animator _mainAnimator;
    [SerializeField] private Animator _firingAnimator;
    [SerializeField] private PlayerAnimatorEventReceiver _eventReceiver;
    
    public override void InstallBindings()
    {
        Container.BindInstance(_firingAnimator).WithId(PlayerAnimatorType.Shooting).AsCached();
        Container.BindInstance(_mainAnimator).WithId(PlayerAnimatorType.Main).AsCached();
        Container.BindInstance(_eventReceiver).AsSingle();
    }
}