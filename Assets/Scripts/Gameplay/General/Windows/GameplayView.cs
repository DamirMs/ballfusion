using Cysharp.Threading.Tasks;
using PT.Tools.EventListener;
using PT.Tools.Windows;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Gameplay.General.Windows
{
    public class GameplayView : MonoBehaviourEventListener
    {
        [SerializeField] private Button pauseButton;
        [SerializeField] private Button menuButton;
        [Space]
        [SerializeField] private GameObject pauseObj;

        [Inject(Id = "Game")] private WindowsManager _windowsManager; 

        private void Awake()
        {
            if (menuButton) menuButton.onClick.AddListener(OpenMenu);
            if (pauseButton) pauseButton.onClick.AddListener(OpenPause);
        }

        private void OpenMenu()
        {
            _windowsManager.CloseAll().Forget();
            _windowsManager.Open(WindowTypeEnum.GameOver).Forget();
                
            GlobalEventBus.On(GlobalEventEnum.GameEnded);
        }
        private void OpenPause()
        {
            _windowsManager.CloseAll().Forget();
            _windowsManager.Open(WindowTypeEnum.GameOver).Forget();
                
            GlobalEventBus.On(GlobalEventEnum.GameEnded);
            
            // GlobalEventBus.On(GlobalEventEnum.GameMenuOpened);
        }
    }
}