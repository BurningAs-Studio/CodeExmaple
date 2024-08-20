using UnityEngine;
using System;

namespace PetWorld.Player
{
    [Serializable]
    public class BaseAnimatorLayer : AnimatorLayer
    {
        private int _lastLocomotionIndex;
        
        private readonly int _horizontalLocomotionValue = Animator.StringToHash("horizontalLocomotion");
        private readonly int _verticalLocomotionValue = Animator.StringToHash("verticalLocomotion");
        private readonly int _locomotionTypeValue = Animator.StringToHash("locomotionType");
        private readonly int _locomotionValue = Animator.StringToHash("locomotion");
		
        private readonly int _changeLocomotionTrigger = Animator.StringToHash("changeLocomotion");
        
        private const int UnarmedLocomotionIndex = 0;
        private const int SwimmingAimLocomotionIndex = 1;
        private const int MainWeaponLocomotionIndex = 2;
        private const int PistolAimLocomotionIndex = 3;
        private const int MainWeaponAimLocomotionIndex = 4;
        
        public void SetLocomotionValue(float value)
        {
            Animator.SetFloat(_locomotionValue, value);
        }

        public void SetLocomotionValue(Vector2 vector)
        {
            Animator.SetFloat(_horizontalLocomotionValue, vector.y);
            Animator.SetFloat(_verticalLocomotionValue, vector.x);
        }
        
        public void ChangeLocomotionTypeToSwimming()
        {
            SetLocomotionType(SwimmingAimLocomotionIndex);
        }
        
        public void ChangeLocomotionTypeToUnarmed()
        {
            ChangeLocomotionIndex(UnarmedLocomotionIndex);
        }
		
        public void ChangeLocomotionTypeToMainWeapon()
        {
            ChangeLocomotionIndex(MainWeaponLocomotionIndex);
        }
		
        public void ChangeLocomotionTypeToPistolAim()
        {
            ChangeLocomotionIndex(PistolAimLocomotionIndex);
        }
		
        public void ChangeLocomotionTypeToMainWeaponAim()
        {
            ChangeLocomotionIndex(MainWeaponAimLocomotionIndex);
        }
		
        private void ChangeLocomotionIndex(int locomotionIndex)
        {
            SetLocomotionType(locomotionIndex);
        }

        private void SetLocomotionType(int index)
        {
            _lastLocomotionIndex = Animator.GetInteger(_locomotionTypeValue);

            Animator.SetInteger(_locomotionTypeValue, index);
            Animator.SetTrigger(_changeLocomotionTrigger);
        }
    }
}