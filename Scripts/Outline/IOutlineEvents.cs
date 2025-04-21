using System;

namespace FAS
{
	public interface IOutlineEvents
	{
		public event Action OnTryDisableOutline;
		public event Action OnTryEnableOutline;
	}
}