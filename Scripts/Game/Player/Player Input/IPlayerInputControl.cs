namespace FAS.Players
{
	public interface IPlayerInputControl
	{
		public void DisableMovementInput();
		
		public void EnableMovementInput();
		
		public void DisableButtonsInput();
		
		public void EnableButtonsInput();
	}
}