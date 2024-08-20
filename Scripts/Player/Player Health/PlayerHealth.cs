using UnityEngine;

namespace PetWorld.Player
{
    public class PlayerHealth : Health
    {
        [SerializeField] private PlayerHealthView _playerHealthView;

        public override void Initialize()
        {
            View = _playerHealthView;
            base.Initialize();
        }

        public override void TakeDamage(int amount)
        {
            base.TakeDamage(amount);
            View.ShowDamageEffect(amount);
        }

        protected override void Heal(int amount)
        {
            base.Heal(amount);
            View.ShowHealEffect(amount);
        }
    }
}