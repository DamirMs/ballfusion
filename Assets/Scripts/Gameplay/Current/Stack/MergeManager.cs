using System;
using Gameplay.Elements;
using Gameplay.Enums;
using Gameplay.IOS.Shop;
using PT.GameplayAdditional.Effects;
using PT.Tools.Effects;
using UnityEngine;
using Zenject;

namespace Gameplay
{
    public class MergeManager : MonoBehaviour
    {
        [SerializeField] private ElementsManager elementsManager;
        [SerializeField] private EffectsSpawner mergeEffectsSpawner;
        [SerializeField] private EffectsSpawner finalMergeEffectsSpawner;
        
        public event Action<ElementType> OnMerge;
        
        [Inject] private ElementProvider _elementProvider;
        [Inject] private ShopManager _shopManager;

        private void Awake()
        {
            mergeEffectsSpawner.Init();
        }
        
        public void MergeElements(IElement elementTo, IElement elementWith)
        {
            if (!elementsManager.ElementIsInGame(elementTo) || 
                !elementsManager.ElementIsInGame(elementWith) || 
                elementTo.ElementType != elementWith.ElementType) return;

            DebugManager.Log(DebugCategory.Gameplay, $"Merge");
            
            if (IsLast(elementTo.ElementType))
            {
                // finalMergeEffectsSpawner.Spawn(elementTo.Transform.position);

                elementsManager.Return(elementTo);
            }
            else
            {
                mergeEffectsSpawner.Spawn(elementTo.Transform.position);

                var nextData = _elementProvider.GetNext(elementTo.ElementType);
                elementTo.SetData(nextData, _shopManager.GetEquippedId(ShopItemEnum.ShopMainItem));
                elementTo.PlayMergeEffect();                
            }

            elementsManager.Return(elementWith);
            
            OnMerge?.Invoke(elementTo.ElementType); //PLAYS ONLY 1 MERGE!
        }

        private bool IsLast(ElementType elementType)
        {
            var values = (ElementType[])Enum.GetValues(typeof(ElementType));
            return elementType == values[values.Length - 1];
        }
    }
}