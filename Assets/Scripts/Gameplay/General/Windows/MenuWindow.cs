using Cysharp.Threading.Tasks;
using PT.Logic.PersistentScene;
using PT.Tools.Windows;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Gameplay.General.Windows
{
    public class MenuWindow : WindowBase
    {
        [SerializeField] private Button[] playButtons;
        [SerializeField] private Button[] settingsButtons;

        [Inject] private SceneLoadManager _sceneLoadManager; 
        [Inject(Id = "Menu")] private WindowsManager _windowsManager; 
        
        private void Awake()
        {
            foreach (var playButton in playButtons) if (playButton) playButton.onClick.AddListener(OnPlayPressed);
            foreach (var settingsButton in settingsButtons) if (settingsButton) settingsButton.onClick.AddListener(OnSettingsPressed);
        }

        private async void OnPlayPressed()
        {
            await UniTask.NextFrame();

            _sceneLoadManager.LoadScene(SceneNameEnum.Game, SceneNameEnum.Menu).Forget();
            _windowsManager.CloseAllFrom(WindowTypeEnum.Menu).Forget();
        }
        
        private void OnSettingsPressed()
        {
            _windowsManager.Open(WindowTypeEnum.MenuSettings).Forget();
        }
    }
}