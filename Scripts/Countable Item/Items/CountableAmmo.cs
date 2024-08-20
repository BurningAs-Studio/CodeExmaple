#if UNITY_EDITOR
using System.Reflection;
#endif
using UnityEngine;
using System;

namespace PetWorld
{
    [Serializable]
    public class CountableAmmo : CountableItem
    {
        [SerializeField] private Ammo _ammo;
		
        public AmmoType Type => _ammo.Type;
		
        public override int Amount
        {
            get => _ammo.Amount;
            protected set => _ammo.Amount = value;
        }
		
#if UNITY_EDITOR
        public void SetType(AmmoType type)
        {
            _ammo = new Ammo(); 
            var ammoType = typeof(Ammo);
            var fieldInfo = ammoType.GetField(
                "<Type>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance);

            if (fieldInfo != null)
                fieldInfo.SetValue(_ammo, type);
            else
                Debug.LogError("Field not found");
        }
#endif
    }
}