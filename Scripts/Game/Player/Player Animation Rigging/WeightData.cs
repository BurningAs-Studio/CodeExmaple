using UnityEngine;

namespace FAS.Players.AnimRig
{
	public class WeightData
	{
		private float _progress;

		private bool _isEnabledLastFrame;
		
		public float Weight { get; private set; }
		
		public bool IsEnableRequested;
		
		public bool IsWeightUpdated(out float newWeight, float duration = 0.1f)
		{
			if (_isEnabledLastFrame != IsEnableRequested)
				_progress = 0;
			
			if (!Mathf.Approximately(_progress, duration))
			{
				_progress = Mathf.MoveTowards(_progress, duration, Time.deltaTime);
				var weightNormalized = Mathf.InverseLerp(0, duration, _progress);
				
				if (IsEnableRequested)
					newWeight = weightNormalized;
				else
					newWeight = 1 - weightNormalized;
				
				if (Mathf.Approximately(Weight, newWeight))
					return false;

				Weight = newWeight;
				return true;
			}

			newWeight = Weight;
			return false;
		}

		public void Reset()
		{
			_isEnabledLastFrame = IsEnableRequested;
			IsEnableRequested = false;
		}
	}
}