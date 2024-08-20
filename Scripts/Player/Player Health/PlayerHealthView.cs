using PetWorld.PopUps;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine;

namespace PetWorld
{
    public class PlayerHealthView : HealthView
    {
        [SerializeField] private Image _changeHealthEffectImage;
        [SerializeField] private Color _damageImageColor;
        [SerializeField] private Color _healImageColor;
        [SerializeField] private float _hideHealthEffectImageDuration = 0.5f;
        [SerializeField] private Ease _hideHealthEffectImageEase;
        [SerializeField] private RectTransform _popUpPoint;
        
        public override void ShowDamageEffect(int amount)
        {
            _changeHealthEffectImage.DOKill();
            _changeHealthEffectImage.color = _damageImageColor;
            _changeHealthEffectImage.DOFade(0, _hideHealthEffectImageDuration)
                .SetEase(_hideHealthEffectImageEase);
            
            NumericPopUp.ShowDamagePopUpScreenSpace(_popUpPoint, amount);
        }

        public override void ShowHealEffect(int amount)
        {
            _changeHealthEffectImage.DOKill();
            _changeHealthEffectImage.color = _healImageColor;
            _changeHealthEffectImage.DOFade(0, _hideHealthEffectImageDuration)
                .SetEase(_hideHealthEffectImageEase);
            
            NumericPopUp.ShowHealPopUpScreenSpace(_popUpPoint, amount);
        }
    }
}