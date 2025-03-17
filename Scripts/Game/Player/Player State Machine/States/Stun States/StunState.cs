using UnityEngine;

namespace FAS.Players.States
{
	public abstract class StunState : PlayerState
	{
		[SerializeField] protected float StunTime = 3f;
        
		protected float ExitTime;
		
		protected abstract PlayerState ExitState { get; }
        
		public override void Enter()
		{
			InputControl.DisableMovementInput();
			InputControl.DisableButtonsInput();
			ExitTime = Time.timeSinceLevelLoad + StunTime;
		}
        
		public override void Perform()
		{
			Jump.ApplyGravity();
            
			if (Time.timeSinceLevelLoad > ExitTime)
				RequestTransition(ExitState);
		}
	}
}