using System;
using System.Linq;
using Gameplay.Configs;
using Gameplay.Enums;
using UnityEngine;

namespace Gameplay.Elements
{
    public class ElementProvider 
    {
        private ElementData[] _elementDatas;
        
        public ElementProvider(ElementsConfig elementsConfig)
        {
            _elementDatas = elementsConfig.ElementDatas;
        }

        public ElementData GetNext(ElementType elementType)
        {
            int current = (int)elementType;
            int upgraded = current * 2;

            if (Enum.IsDefined(typeof(ElementType), upgraded))
            {
                return _elementDatas.First(x => (int)x.ElementType == upgraded);
            }
                
            DebugManager.Log(DebugCategory.Errors, $"Max upgrade reached or invalid upgrade value {elementType}", LogType.Error);
            return null;
        }

        public ElementData GetData(ElementType elementType)
        {
            return _elementDatas.First(x => x.ElementType == elementType);
        }
    }
}