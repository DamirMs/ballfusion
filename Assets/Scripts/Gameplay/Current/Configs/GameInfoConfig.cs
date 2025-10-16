using Gameplay.Enums;
using Gameplay.General.Configs;
using UnityEngine;

namespace Gameplay.Current.Configs
{
    [CreateAssetMenu(menuName = "Configs/GameInfo", fileName = "GameInfoConfig")]
    public class GameInfoConfig : BaseGameInfoConfig
    {
        [SerializeField] private float dropWaitDelay = 0.1f;
        [SerializeField] private ElementType[] playableElementTypes;
        
        public float DropWaitDelay => dropWaitDelay;
        public ElementType[] PlayableElementTypes => playableElementTypes;
        
    }
}