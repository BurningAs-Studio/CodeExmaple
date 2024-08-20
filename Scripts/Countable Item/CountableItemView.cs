using UnityEngine;
using TMPro;

namespace PetWorld
{
    public class CountableItemView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _amountText;

        public void UpdateAmount(int amount)
        {
            _amountText.text = amount.ToString();
        }
    }
}