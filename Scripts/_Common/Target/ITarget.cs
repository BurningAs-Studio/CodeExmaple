namespace FAS
{
	public enum TargetType
	{
		Player,
		FakePlayer,
		Zombie,
		Other
	}
	
	public interface ITarget : IReadOnlyTarget
	{
		public void TakeDamage(float damage, DamageReceiver damageDealer);
	}
}