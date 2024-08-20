using UnityEngine;
using Zenject;
using System;

namespace PetWorld.Player
{
    [Serializable]
    public class PlayerWeaponAnimator
    {
        [Inject] [NonSerialized] private BaseAnimatorLayer _baseAnimatorLayer;
        [Inject] [NonSerialized] private PlayerAnimationRig _animationRig;

        public void SetWeaponAnimation(WeaponAnimType animType, Transform leftHandPoint)
        {
            switch (animType)
            {
                case WeaponAnimType.Pistol:
                    _baseAnimatorLayer.ChangeLocomotionTypeToUnarmed();
                    _animationRig.DisableHandsRig();
                    break;
                case WeaponAnimType.Rifle:
                    _baseAnimatorLayer.ChangeLocomotionTypeToMainWeapon();
                    _animationRig.LeftHandToTarget(leftHandPoint);
                    break;
                case WeaponAnimType.Shotgun:
                    _baseAnimatorLayer.ChangeLocomotionTypeToMainWeapon();
                    _animationRig.LeftHandToTarget(leftHandPoint);
                    break;
                default:
                    _baseAnimatorLayer.ChangeLocomotionTypeToUnarmed();
                    _animationRig.DisableHandsRig();
                    break;
            }
        }

        public void SetWeaponAimAnimation(WeaponAnimType animType, Transform leftHandPoint)
        {
            Debug.Log("!");
            switch (animType)
            {
                case WeaponAnimType.Pistol:
                    _baseAnimatorLayer.ChangeLocomotionTypeToPistolAim();
                    _animationRig.LeftHandToTarget(leftHandPoint);
                    break;
                case WeaponAnimType.Rifle:
                    _baseAnimatorLayer.ChangeLocomotionTypeToMainWeaponAim();
                    _animationRig.LeftHandToTarget(leftHandPoint);
                    break;
                case WeaponAnimType.Shotgun:
                    _baseAnimatorLayer.ChangeLocomotionTypeToMainWeaponAim();
                    _animationRig.LeftHandToTarget(leftHandPoint);
                    break;
                default:
                    _baseAnimatorLayer.ChangeLocomotionTypeToUnarmed();
                    _animationRig.DisableHandsRig();
                    break;
            }
        }
    }
}