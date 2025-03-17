namespace FAS.Players
{
	public interface IReadOnlyPlayerWeapon
	{
		public IWeaponData Data { get; }
		
		public bool IsChangedThisFrame { get; }
		public bool IsHasWeapon { get; }
	}
}