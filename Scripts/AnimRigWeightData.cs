using UnityEngine;

namespace FAS
{
	public class AnimRigWeightData
	{
		private float _requestedWeight;
		private float _weight;
        
		private bool _isEnableRequested;
		
		public void RequestEnable(float weight = 1)
		{
			_requestedWeight = weight;
			_isEnableRequested = true;
		}

		public bool IsWeightUpdated(out float newWeight, float duration = 0.1f)
		{
			var oldWeight = _weight;
			var target = _isEnableRequested ? _requestedWeight : 0f;
			var step = Time.deltaTime / duration;

			_weight = Mathf.MoveTowards(oldWeight, target, step);

			newWeight = _weight;
			_isEnableRequested = false;
			return !Mathf.Approximately(oldWeight, newWeight);
		}
	}
}