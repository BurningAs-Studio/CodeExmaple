using System;

namespace FAS
{
	[Serializable]
	public struct DamageableData
	{
		public DamageableType Type;
		public BodyPart BodyPart;

		public DamageableData(DamageableType type, BodyPart bodyPart)
		{
			Type = type;
			BodyPart = bodyPart;
		}
	}
}