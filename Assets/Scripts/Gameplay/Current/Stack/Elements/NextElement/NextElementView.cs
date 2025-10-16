using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.Elements.NextElement
{
    public class NextElementView : MonoBehaviour
    {
        [SerializeField] private Image nextElementImage;
        [SerializeField] private TextMeshProUGUI nextElementValue;

        public void Set(Sprite nextElementSprite, int elementValue)
        {
            if (nextElementSprite) nextElementImage.sprite = nextElementSprite;
            nextElementValue.text = elementValue.ToString();
        }
    }
}