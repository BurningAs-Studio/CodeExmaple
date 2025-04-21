namespace FAS.Pickups
{
	public interface IPickupVisitable
	{
		public void Apply(IPickupVisitor visitor);
	}
}