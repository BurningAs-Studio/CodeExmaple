namespace FAS.Pickups
{
	public interface IPickupVisitor
	{
		public void Visit(HealthBox healthBox);
		
		public void Visit(AmmoBox ammoBox);
	}
}