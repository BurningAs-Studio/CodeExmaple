using UnityEngine;
using System;

namespace FAS.Fatality
{
	public interface IReadOnlyFatalityTarget
	{
		public FatalityData CurrentData { get; }
		
		public Vector3 Position {get;}
		
		public bool IsReadyToFatality { get; }
		
		public event Action<IFatalityTarget> OnNotReadyToFatality;
		public event Action<IFatalityTarget> OnReadyToFatality;
		public event Action OnReceivedFatality;
		public event Action OnPerformFatality;
		public event Action OnFinishFatality;
	}
}