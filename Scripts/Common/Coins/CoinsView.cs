using FAS.Save;
using FAS.UI;
using UnityEngine;
using Zenject;
using TMPro;

namespace FAS
{
    public class CoinsView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _coinsAmountText;
        [SerializeField] private CustomButton _plusButton;
        
        private void OnEnable()
        {
            SaveData.OnCoinsAmountChanged += UpdateView;
        }
        
        private void OnDisable()
        {
            SaveData.OnCoinsAmountChanged -= UpdateView;
        }

        private void Start()
        {
            UpdateView();
        }

        private void UpdateView()
        {
            _coinsAmountText.text = SaveData.CoinsAmount.ToString();
        }

        public void UpdateView(string amountText)
        {
            _coinsAmountText.text = amountText;
        }
    }
}