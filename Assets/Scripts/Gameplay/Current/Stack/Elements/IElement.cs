using System;
using Gameplay.Enums;
using UnityEngine;

namespace Gameplay.Elements
{
    public interface IElement
    {
        Transform Transform { get; }
        ElementType ElementType { get; }
        bool IsFalling { get; }

        void Init(ElementData elementData, Action<IElement, IElement> mergeAction, int skinIndex);
        void SetData(ElementData elementData, int skinIndex);
        void Drop();
        void PlayMergeEffect();
        void SetActive(bool active);
    }
}