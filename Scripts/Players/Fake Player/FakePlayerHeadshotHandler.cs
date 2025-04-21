using FAS.Players;
using Zenject;

namespace FAS.FakePlayers
{
	public class FakePlayerHeadshotHandler : PlayerHeadshotHandler
	{
		[Inject] private IHeadshotView _view;
		
		protected override void OnHeadshot()
		{
			base.OnHeadshot();
			_view.ShowView();
		}
	}
}