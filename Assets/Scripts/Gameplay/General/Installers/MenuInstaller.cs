using PT.Tools.Windows;
using Zenject;

namespace Gameplay.General.Installers
{
    public class MenuInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<WindowsManager>().WithId("Menu").FromComponentInHierarchy().AsSingle();
        }
    }
}