using UnityEngine;

namespace FAS
{
	public interface IReadOnlyTarget
	{
		public TargetType Type { get; }
		
		public Vector3 HeadPosition { get; }
		
		public Vector3 Position { get; }
	}
}