using FAS.Players.States;
using Cinemachine;
using UnityEngine;
using VInspector;
using Zenject;
using FAS.UI;

namespace FAS.Players
{
	public class PlayerCameraInstaller : MonoInstaller
	{
		[SerializeField] private CinemachineVirtualCamera _fatalityCamera;
		[SerializeField] private CinemachineVirtualCamera _aimCamera;
		[SerializeField] private PlayerCameraInputPanel _inputPanel;
		[SerializeField] private CinemachineFreeLook _followCamera;
		[SerializeField] private PlayerCameraRotator _rotator;
		[SerializeField] private PlayerCameraShaker _shaker;
		[SerializeField] private CamerasSwitcher _switcher;
		[SerializeField] private Camera _mainCamera;
		
		public override void InstallBindings()
		{
			Container.BindInstance(_fatalityCamera)
				.WithId(VirtualCameraType.Fatality)
				.WhenInjectedIntoInstance(_switcher);
			
			Container.BindInstance(_fatalityCamera)
				.WithId(VirtualCameraType.Fatality)
				.WhenInjectedIntoInstance(_rotator);
			
			Container.BindInstance(_aimCamera)
				.WithId(VirtualCameraType.Aim)
				.WhenInjectedIntoInstance(_switcher);
			
			Container.BindInstance(_aimCamera)
				.WithId(VirtualCameraType.Aim)
				.WhenInjectedIntoInstance(_rotator);
			
			Container.BindInstance(_followCamera).WhenInjectedIntoInstance(_switcher);
			Container.BindInstance(_followCamera).WhenInjectedIntoInstance(_rotator);
			
			Container.BindInstance(_inputPanel).WhenInjectedInto<CameraPanelSyncButton>();
			Container.BindInstance(_rotator).WhenInjectedInto<PlayerState>();

			Container.Bind<IReadOnlyPlayerCameraInputPanel>().FromInstance(_inputPanel).AsSingle();
			Container.BindInstance(_mainCamera).AsSingle();
			Container.BindInstance(_switcher).AsSingle();
			Container.BindInstance(_shaker).AsSingle();
		}

#if UNITY_EDITOR
		[Button]
		private void FindDependencies()
		{
			_mainCamera = GetComponentInChildren<CinemachineBrain>(true).GetComponent<Camera>();
			_inputPanel = GetComponentInChildren<PlayerCameraInputPanel>(true);
			_followCamera = GetComponentInChildren<CinemachineFreeLook>(true);
			_rotator = GetComponentInChildren<PlayerCameraRotator>(true);
			_shaker = GetComponentInChildren<PlayerCameraShaker>(true);
			_switcher = GetComponentInChildren<CamerasSwitcher>(true);
		}
#endif
	}
}