using Gameplay.Elements;
using UnityEngine;

namespace Gameplay.Configs
{
    [CreateAssetMenu(menuName = "Configs/ElementsConfig", fileName = "ElementsConfig")]
    public class ElementsConfig : ScriptableObject
    {
        [SerializeField] private ElementData[] elementDatas;
        
        public ElementData[] ElementDatas => elementDatas;
    }
}