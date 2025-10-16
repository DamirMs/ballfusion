using System;
using Gameplay.Enums;
using UnityEngine;

namespace Gameplay.Elements
{
    [Serializable]
    public class ElementData
    {
        [SerializeField] private ElementType elementType;
        [SerializeField] private Sprite[] sprites;
        [SerializeField] private Color color = Color.white;
        [SerializeField] private Sprite showcaseSprite;
        
        public ElementType ElementType => elementType;
        public Sprite[] Sprites => sprites;
        public Color Color => color;
        public Sprite ShowcaseSprite => showcaseSprite;
    }
}