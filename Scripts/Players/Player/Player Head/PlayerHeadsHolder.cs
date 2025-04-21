using System.Collections.Generic;

namespace FAS.Players
{
	public class PlayerHeadsHolder
	{
		private readonly Stack<PlayerSkinData> _heads = new ();

		public int Count => _heads.Count;

		public bool TryRemove(out PlayerSkinData head)
		{
			if (_heads.Peek())
			{
				head = _heads.Pop();
				return true;
			}

			head = null;
			return false;
		}
		
		public void Add(PlayerSkinData data) => _heads.Push(data);
	}
}