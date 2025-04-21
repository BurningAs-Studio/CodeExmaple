using UnityEngine;
using VInspector;
using System;

namespace FAS.Fatality
{
	public class FatalityTarget : MonoBehaviour, IFatalityTarget, IFatalityReadiness
	{
		[SerializeField] private Transform _view;
		
		private readonly Vector3 _viewOffset = Vector3.up * 2f;
		
		public FatalityData CurrentData { get; private set; }
		
		public Vector3 Position => transform.position;
		
		public bool IsReadyToFatality { get; private set; }
		
		public event Action<IFatalityTarget> OnNotReadyToFatality;
		public event Action<IFatalityTarget> OnReadyToFatality;
		public event Action OnReceivedFatality;
		public event Action OnPerformFatality;
		public event Action OnFinishFatality;

		private void Awake()
		{
			_view.SetParent(null);
		}

		public void StartFatality(FatalityData fatalityData)
		{
			CurrentData = fatalityData;
			OnReceivedFatality?.Invoke();
			SetNotReadyToFatality();
		}

		public void PerformFatality() => OnPerformFatality?.Invoke();
		
		public void FinishFatality() => OnFinishFatality?.Invoke();

#if UNITY_EDITOR
		[Button]
#endif
		public void SetReadyToFatality()
		{
			IsReadyToFatality = true;
			_view.gameObject.SetActive(true);
			OnReadyToFatality?.Invoke(this);
		}

#if UNITY_EDITOR
		[Button]
#endif
		public void SetNotReadyToFatality()
		{
			IsReadyToFatality = false;
			_view.gameObject.SetActive(false);
			OnNotReadyToFatality?.Invoke(this);
		}

		private void Update()
		{
			if (IsReadyToFatality)
				_view.position = transform.position + _viewOffset;
		}
	}
}