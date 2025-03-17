using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;
using System;
using FAS.UI;

namespace FAS.LootBoxes
{
    public class LootSpinScreen : UIInteractableScreen
    {
        [SerializeField] private Transform _spinView;
        [SerializeField] private List<Transform> _spinPoints;
        [SerializeField] private List<LootBoxItemData> _itemDatas;
        [SerializeField] private LootBoxItem _itemPrefab;
        [SerializeField] private CustomButton _continueButton;
        [SerializeField] private float _spinDuration = 3f;
        [SerializeField] private int _fullRotations = 3;
        [SerializeField] private Ease _spinEase;

        private readonly List<LootBoxItem> _items = new();
        
        private LootBoxItem _winnerItem;
        
        private float _offset;
        
        private bool _isSpinning;

        private const int WINNER_STOP_INDEX = 3;
        
        public event Action<LootBoxItemData> OnLootSelected;
        
        private void OnEnable()
        {
            _continueButton.OnClick.AddListener(StartSpin);
        }

        private void OnDisable()
        {
            _continueButton.OnClick.RemoveListener(StartSpin);
        }

        private void Start()
        {
            CreateItems();
            _offset = 0f;
            RepositionItems();
        }

        public override void Show()
        {
	        base.Show();
	        _continueButton.gameObject.SetActive(true);
        }

        private void CreateItems()
        {
            foreach (var data in _itemDatas)
            {
                var item = Instantiate(_itemPrefab, _spinView);
                item.Initialize(data);
                item.gameObject.SetActive(false);
                _items.Add(item);
            }
        }

        private void StartSpin()
        {
            if (_isSpinning) return;

            _continueButton.gameObject.SetActive(false);
            _winnerItem = DetermineWinner();
            _isSpinning = true;

            var winnerIndex = _items.IndexOf(_winnerItem);

            var baseOffset = Mathf.FloorToInt(_offset);
            var currentMod = baseOffset % _items.Count;
            if (currentMod < 0) currentMod += _items.Count;

            var desiredMod = (winnerIndex + WINNER_STOP_INDEX) % _items.Count;
            var delta = (desiredMod - currentMod + _items.Count) % _items.Count;
            var finalIntOffset = baseOffset + delta + _fullRotations * _items.Count;

            DOTween.To(
                () => _offset,
                x => _offset = x,
                finalIntOffset,
                _spinDuration
            )
            .SetEase(_spinEase)
            .OnUpdate(RepositionItems)
            .OnComplete(() =>
            {
                _isSpinning = false;
                _offset = finalIntOffset;
                RepositionItems();
                OnLootSelected?.Invoke(_winnerItem.Data);
            });
        }

        private LootBoxItem DetermineWinner()
        {
            var total = _itemDatas.Sum(data => data.DropChance);
            var randomValue = UnityEngine.Random.Range(0f, total);
            var cumulative = 0f;
            foreach (var data in _itemDatas)
            {
                cumulative += data.DropChance;
                if (randomValue <= cumulative)
                    return _items.Find(i => i.Data.Type == data.Type);
            }
            return _items[0];
        }

        private void RepositionItems()
        {
            var baseOffset = Mathf.FloorToInt(_offset);
            var fraction = _offset - baseOffset;

            var usedIndices = new HashSet<int>();

            for (var pointIdx = 0; pointIdx < _spinPoints.Count; pointIdx++)
            {
                var itemIndex = (baseOffset - pointIdx + _items.Count * 10) % _items.Count;

                var item = _items[itemIndex];
                var p0Index = pointIdx;
                var p1Index = (pointIdx + 1) % _spinPoints.Count;

                var p0 = _spinPoints[p0Index].position;
                var p1 = _spinPoints[p1Index].position;

                var t = fraction;

                if (p1Index < p0Index && t > 0)
                {
                    item.gameObject.SetActive(false);
                    continue;
                }

                item.gameObject.SetActive(true);
                item.SetPosition(Vector3.Lerp(p0, p1, t));
                usedIndices.Add(itemIndex);
            }

            for (var i = 0; i < _items.Count; i++)
            {
                if (!usedIndices.Contains(i))
                    _items[i].gameObject.SetActive(false);
            }
        }
    }
}
