using Gameplay.Current.Configs;
using Gameplay.Enums;
using Gameplay.General.Configs;
using UnityEngine;
using Zenject;

namespace Gameplay.Elements.NextElement
{
    public class NextElementManager : MonoBehaviour
    {
        [SerializeField] private NextElementView nextElementView;
        
        [Inject] private ElementProvider _elementProvider;
        [Inject] private GameInfoConfig _gameInfoConfig;

        public ElementData GetNext()
        {
            var type = _gameInfoConfig.PlayableElementTypes.GetRandomElement();
            var data = _elementProvider.GetData(type);
            
            SetNext(data);
            
            return data;
        }
        
        private void SetNext(ElementData data)
        {
            // nextElementView.Set(data.ShowcaseSprite, (int)data.ElementType);
        }
    }
}