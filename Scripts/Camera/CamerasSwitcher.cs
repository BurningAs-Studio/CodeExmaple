using Cinemachine;
using FAS.Players;
using UnityEngine;
using Zenject;

namespace FAS
{
	public class CamerasSwitcher : MonoBehaviour
	{
		[Inject (Id = VirtualCameraType.Fatality)] private CinemachineVirtualCamera _fatalityCamera;
		[Inject (Id = VirtualCameraType.Aim)] private CinemachineVirtualCamera _aimCamera;
		[Inject] private CinemachineFreeLook _followCamera;
		
		private VirtualCameraType _requestedCamera;
		
		private float _resetCameraTimer;

		private const float AIM_TO_FOLLOW_BLEND_TIME = 0.15f;
		
		private const float DEFAULT_ORBIT_TOP_RIG_HEIGHT = 3.71f;
		private const float DEFAULT_ORBIT_MIDDLE_RIG_HEIGHT = 2.5f;
		private const float DEFAULT_ORBIT_BOTTOM_RIG_HEIGHT = 1f;

		private const float DEFAULT_ORBIT_TOP_RADIUS = 2.51f;
		private const float DEFAULT_ORBIT_MIDDLE_RADIUS = 6f;
		private const float DEFAULT_ORBIT_BOTTOM_RADIUS = 4f;

		public VirtualCameraType CurrentCamera { get; private set; } = VirtualCameraType.Follow;
		
		public bool IsCameraRestarted { get; private set; } = true;
		
		public void DisableFatalityCamera() => _fatalityCamera.gameObject.SetActive(false);
		
		public void EnableFatalityCamera() => _fatalityCamera.gameObject.SetActive(true);

		public void PlayFollowCamera()
		{
			if (CurrentCamera != VirtualCameraType.Follow)
			{
				_requestedCamera = VirtualCameraType.Follow;
				FollowCameraOrbitsToDefault();
				IsCameraRestarted = false;
				_aimCamera.gameObject.SetActive(false);	
			}
		}

		public void PlayAimCamera()
		{
			if (CurrentCamera != VirtualCameraType.Aim)
			{
				_requestedCamera = VirtualCameraType.Aim;
				var lastFollowCameraPosition = _followCamera.transform.position;
				IsCameraRestarted = false;
				FollowCameraOrbitsToDefault();
				_aimCamera.gameObject.SetActive(true);
				_followCamera.transform.position = lastFollowCameraPosition;	
			}
		}

		private void FollowCameraOrbitsToDefault()
		{
			_followCamera.m_Orbits[0].m_Height = DEFAULT_ORBIT_TOP_RIG_HEIGHT;
			_followCamera.m_Orbits[1].m_Height = DEFAULT_ORBIT_MIDDLE_RIG_HEIGHT;
			_followCamera.m_Orbits[2].m_Height = DEFAULT_ORBIT_BOTTOM_RIG_HEIGHT;
			
			_followCamera.m_Orbits[0].m_Radius = DEFAULT_ORBIT_TOP_RADIUS;
			_followCamera.m_Orbits[1].m_Radius = DEFAULT_ORBIT_MIDDLE_RADIUS;
			_followCamera.m_Orbits[2].m_Radius = DEFAULT_ORBIT_BOTTOM_RADIUS;
		}
		
		private void Update()
		{
			if (!IsCameraRestarted)
			{
				_resetCameraTimer += Time.deltaTime;
				if (_resetCameraTimer > AIM_TO_FOLLOW_BLEND_TIME)
				{
					_resetCameraTimer = 0;
					CurrentCamera = _requestedCamera;
					IsCameraRestarted = true;
				}
			}
		}
	}
}