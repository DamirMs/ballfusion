using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.Elements
{
    [Serializable]
    public class ElementView
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private TextMeshProUGUI levelText;
        [Space]        
        [SerializeField] private Transform mainTransform;
        [SerializeField] private Vector2 scaleAddition;
        [SerializeField] private float scaleInDuration = 0.1f;
        [SerializeField] private float scaleOutDuration = 0.2f;

        private Vector2 _initialScale;

        private Tween _scaleTween;

        public void Init()
        {
            _initialScale = mainTransform.localScale;
        }
        
        public void Set(Sprite sprite, Color color, int level)
        {
            spriteRenderer.sprite = sprite;
            spriteRenderer.color = color;
            // levelText.text = level.ToString();
            
            // mergeImage.enabled = false;

            StopScale();
        } 
        
        public void PlayMerge()
        {
            StopScale();

            // mainTransform.localScale = _initialScale;
            //
            // _scaleTween = mainTransform.DOScale(_initialScale - scaleAddition, scaleInDuration)
            //     .SetEase(Ease.OutQuad)
            //     .OnComplete(() =>
            //     {
            //         _scaleTween = mainTransform.DOScale(_initialScale, scaleOutDuration)
            //             .SetEase(Ease.OutBack)
            //             .OnComplete(StopScale);
            //     });
        }

        private void StopScale()
        {
            _scaleTween?.Kill();
        }
    }
}