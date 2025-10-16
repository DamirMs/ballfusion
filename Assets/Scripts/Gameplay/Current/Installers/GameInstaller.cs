using Gameplay.Configs;
using Gameplay.Elements;
using Gameplay.Elements.NextElement;
using Gameplay.General.Installers;
using UnityEngine;

namespace Gameplay.Current.Installers
{
    public class GameInstaller : BaseGameInstaller
    {
        [SerializeField] private ElementsConfig elementsConfig; 

        public override void InstallBindings()
        {
            base.InstallBindings();
            
            Container.Bind<DropController>().FromComponentInHierarchy().AsSingle();
            Container.Bind<NextElementManager>().FromComponentInHierarchy().AsSingle();
            Container.Bind<ElementsManager>().FromComponentInHierarchy().AsSingle();
            
            Container.Bind<ElementsConfig>().FromInstance(elementsConfig).AsSingle();
            Container.BindInterfacesAndSelfTo<ElementProvider>().AsSingle();
        }
    }
}