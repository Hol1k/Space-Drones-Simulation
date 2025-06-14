using System.Collections.Generic;
using Game.Drones.Scripts;
using UnityEngine;
using Zenject;

namespace Game.Scripts
{
    public class DroneInstaller : MonoInstaller
    {
        [SerializeField] private Drone _dronePrefab;
        
        private List<Transform> _occupiedResources = new List<Transform>();
        
        public override void InstallBindings()
        {
            Container.Bind<List<Transform>>().FromInstance(_occupiedResources).AsSingle();
            
            Container.BindFactory<Drone, Drone.Factory>()
                .FromComponentInNewPrefab(_dronePrefab);
        }
    }
}