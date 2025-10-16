using Gameplay.Enums;
using TMPro;
using UnityEngine;

namespace Gameplay.UI
{
    public class ElementTextView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI text;
        
        private ElementType _currentElementType;

        private void OnDisable()
        {
            _currentElementType = ElementType.Empty;
        }
        
        public void Set(ElementType elementType)
        {
            if (elementType != _currentElementType)
            {
                _currentElementType = elementType;
                text.text = ((int)elementType).ToString();
            }
        }
    }
}