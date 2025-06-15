using Game.Drones.Scripts;
using Game.SpaceResources.Scripts;
using Zenject;

namespace Game.UI.Scripts
{
    public class UIInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<UIInputController>().FromComponentInHierarchy().AsSingle();
            
            Container.BindInterfacesAndSelfTo<ResourceSpawner>().FromComponentInHierarchy().AsSingle();
            
            Container.BindInterfacesAndSelfTo<Drone>().FromComponentsInHierarchy().AsTransient();
        }
    }
}