using FAS.Weapons;

namespace FAS.Players
{
	public interface IReadOnlyPlayerWeapon
	{
		public IReadOnlyWeapon CurrentWeapon { get; }
	}
}