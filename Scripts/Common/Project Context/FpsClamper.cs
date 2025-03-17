using UnityEngine;

namespace FAS
{
	public class FpsClamper : MonoBehaviour
	{
		[SerializeField] private int _targetFps = 60;
		
		private void Start()
		{
			Application.targetFrameRate = _targetFps;
		}
	}
}