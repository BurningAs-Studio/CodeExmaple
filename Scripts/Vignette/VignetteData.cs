using DG.Tweening;
using UnityEngine;
using System;

namespace FAS
{
	[Serializable]
	public struct VignetteData
	{
		[Tooltip("Dont make alpha > 0")]
		public Color Color;
		public float MaxAlphaNormalized;
		public float EnableDelay;
		public float EnableTime;
		public Ease EnableEase;
		public float PlayTime;
		public float DisableTime;
		public Ease DisableEase;
	}
}