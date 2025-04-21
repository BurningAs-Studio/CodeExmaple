using UnityEngine;
using Zenject;

namespace FAS
{
	public class OutlineLayerSwitcher : MonoBehaviour
	{
		[InjectOptional] private IOutlineEvents _events;
		
		private int _defaultLayerIndex;

		private bool _isOutlining;
		
		private const int OUTLINE_LAYER_INDEX = 15;
		
		private void Awake()
		{
			_defaultLayerIndex = gameObject.layer;
		}
		
		private void Start()
		{
			if (_events == null)
			{
				Destroy(this);
			}
			else
			{
				_events.OnTryDisableOutline += TryDisableOutline;
				_events.OnTryEnableOutline += TryEnableOutline;
			}
		}
		
		private void OnDestroy()
		{
			if (_events != null)
			{
				_events.OnTryDisableOutline -= TryDisableOutline;
				_events.OnTryEnableOutline -= TryEnableOutline;
			}
		}
		
		private void TryEnableOutline()
		{
			if (!_isOutlining)
			{
				gameObject.layer = OUTLINE_LAYER_INDEX;
				_isOutlining = true;
			}
		}
		
		private void TryDisableOutline()
		{
			if (_isOutlining)
			{
				gameObject.layer = _defaultLayerIndex;
				_isOutlining = false;
			}
		}
	}
}