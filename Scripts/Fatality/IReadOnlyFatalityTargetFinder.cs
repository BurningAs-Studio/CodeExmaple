using System;

namespace FAS.Fatality
{
	public interface IReadOnlyFatalityTargetFinder
	{
		public bool IsHasTarget { get; }

		public event Action OnFindFirstTarget;
		public event Action OnLoseAllTarget;
	}
}