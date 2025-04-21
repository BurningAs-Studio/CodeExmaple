using UnityEngine;
using System;

namespace FAS
{
	public class OutlineEventsSender : MonoBehaviour, IOutlineEventsSender
	{
		public event Action OnTryDisableOutline;
		public event Action OnTryEnableOutline;

		public void TryDisableOutline() => OnTryDisableOutline?.Invoke();

		public void TryEnableOutline() => OnTryEnableOutline?.Invoke();
	}
}