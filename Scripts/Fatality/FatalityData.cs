using UnityEngine;
using System;

namespace FAS.Fatality
{
	[Serializable]
	public struct FatalityData
	{
		public FatalityType Type;
		public Transform TargetPoint;
		public ParticleSystem StartFatalityEffect;
		
		public Quaternion Rotation => TargetPoint.rotation;
		public Vector3 Position => TargetPoint.position;
	}
}