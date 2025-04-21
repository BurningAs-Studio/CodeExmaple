using UnityEngine.Rendering.Universal;
using FAS.Players;
using UnityEngine;
using Zenject;

namespace FAS
{
	public enum GraphicsQualityType
	{
		Low,
		Normal,
		High,
		Ultra
	}
	
	public class Settings : MonoBehaviour
	{
		[SerializeField] private UniversalRenderPipelineAsset _lowQualityAsset;
		[SerializeField] private UniversalRenderPipelineAsset _normalQualityAsset;
		[SerializeField] private UniversalRenderPipelineAsset _highQualityAsset;
		[SerializeField] private UniversalRenderPipelineAsset _ultraQualityAsset;
		[SerializeField] private ParticleSystem _crunchCrowsEffect;

		[Inject] private PlayerUIScreensSwitcher _uiScreensSwitcher;
		[Inject] private IReadOnlyPlayerInputEvents _inputEvents;
		[Inject] private SettingsView _view;
		
		private GraphicsQualityType _currentGraphicsQuality;

		private int _currentFPSLimit;

		public static float CurrentCameraSensitivity { get; private set; }
		
		private const int FPS_120 = 120;
		private const int FPS_90 = 90;
		private const int FPS_60 = 60;
		private const int FPS_30 = 30;
		
		private const float DEFAULT_CAMERA_SENSITIVITY = 10f;
		private const float MIN_CAMERA_SENSITIVITY = 1f;
		private const float MAX_CAMERA_SENSITIVITY = 15f;

		private void Awake()
		{
			CurrentCameraSensitivity = DEFAULT_CAMERA_SENSITIVITY;
			_view.Initialize(MIN_CAMERA_SENSITIVITY, MAX_CAMERA_SENSITIVITY, CurrentCameraSensitivity);

#if UNITY_ANDROID
			SetNormalQuality();
#elif UNITY_IOS
			SetHighQuality();
#endif
			
			Set60FPS();
		}

		private void OnEnable()
		{
			_inputEvents.OnSettingsButtonClicked += _uiScreensSwitcher.ShowSettingsScreen;
			_view.OnCloseButtonClicked += _uiScreensSwitcher.ShowDefaultScreen;
			_view.OnSensitivitySliderValueChanged += SetSensitivity;
			_view.OnNormalQualityButtonClicked += SetNormalQuality;
			_view.OnUltraQualityButtonClicked += SetUltraQuality;
			_view.OnHighQualityButtonClicked += SetHighQuality;
			_view.OnLowQualityButtonClicked += SetLowQuality;
			_view.On120FPSButtonClicked += Set120FPS;
			_view.On90FPSButtonClicked += Set90FPS;
			_view.On60FPSButtonClicked += Set60FPS;
			_view.On30FPSButtonClicked += Set30FPS;
		}

		private void OnDisable()
		{
			_inputEvents.OnSettingsButtonClicked -= _uiScreensSwitcher.ShowSettingsScreen;
			_view.OnCloseButtonClicked -= _uiScreensSwitcher.ShowDefaultScreen;
			_view.OnSensitivitySliderValueChanged -= SetSensitivity;
			_view.OnNormalQualityButtonClicked -= SetNormalQuality;
			_view.OnUltraQualityButtonClicked -= SetUltraQuality;
			_view.OnHighQualityButtonClicked -= SetHighQuality;
			_view.OnLowQualityButtonClicked -= SetLowQuality;
			_view.On120FPSButtonClicked -= Set120FPS;
			_view.On90FPSButtonClicked -= Set90FPS;
			_view.On60FPSButtonClicked -= Set60FPS;
			_view.On30FPSButtonClicked -= Set30FPS;
		}
		
		private void SetSensitivity(float value) => CurrentCameraSensitivity = value;

		private void SetLowQuality()
			=> ChangeGraphicsQuality(_lowQualityAsset, GraphicsQualityType.Low, false);
		
		private void SetNormalQuality()
			=> ChangeGraphicsQuality(_normalQualityAsset, GraphicsQualityType.Normal, false);
		
		private void SetHighQuality()
			=> ChangeGraphicsQuality(_highQualityAsset, GraphicsQualityType.High, true);

		
		private void SetUltraQuality()
			=> ChangeGraphicsQuality(_ultraQualityAsset, GraphicsQualityType.Ultra, true);


		private void ChangeGraphicsQuality(UniversalRenderPipelineAsset qualityAsset, GraphicsQualityType qualityType,
			bool enableCrowsEffect)
		{
			QualitySettings.SetQualityLevel(GetQualityIndexByAssetName(qualityAsset.name), true);

			if (_crunchCrowsEffect != null)
			{
				if (enableCrowsEffect)
				{
					_crunchCrowsEffect.gameObject.SetActive(true);
					_crunchCrowsEffect.Play();
				}
				else
				{
					_crunchCrowsEffect.gameObject.SetActive(false);
				}	
			}
			
			_currentGraphicsQuality = qualityType;
			_view.UpdateGraphicsQualityButtonsState(_currentGraphicsQuality);
		}
		
		private int GetQualityIndexByAssetName(string assetName)
		{
			for (int i = 0; i < QualitySettings.names.Length; i++)
				if (QualitySettings.names[i].ToLower().Contains(assetName.ToLower()))
					return i;
			
			return -1;
		}
		
		private void Set120FPS() => SetFPSLimit(FPS_120);
		
		private void Set90FPS() => SetFPSLimit(FPS_90);
		
		private void Set60FPS() => SetFPSLimit(FPS_60);
		
		private void Set30FPS() => SetFPSLimit(FPS_30);
		
		private void SetFPSLimit(int fps)
		{
			_currentFPSLimit = fps;
			Application.targetFrameRate = _currentFPSLimit;
			_view.UpdateFPSLimitButtonsState(_currentFPSLimit);
		}
	}
}