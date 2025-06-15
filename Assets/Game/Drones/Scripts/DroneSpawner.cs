using System;
using Game.UI.Scripts;
using UnityEngine;
using Zenject;

namespace Game.Drones.Scripts
{
    public class DroneSpawner : MonoBehaviour
    {
        public DroneTeam team;
        
        [SerializeField] private GameObject dronePrefab;
        private Drone.Factory _droneFactory;

        UIInputController _uiInputController;
        private int _currentDroneCount = 0;

        [SerializeField] [Range(0, 5)] private int maxDronesCount;
        private Drone[] _dronesArray;

        [SerializeField] private GameObject resourcesParent;
        
        [Inject]
        private void Construct(Drone.Factory droneFactory, UIInputController uiInputController)
        {
            _droneFactory = droneFactory;
            _uiInputController = uiInputController;
        }

        private void Awake()
        {
            maxDronesCount = _uiInputController.maxDroneCount;
        }

        private void Start()
        {
            _dronesArray = CreateDroneObjects(maxDronesCount);
        }

        private void Update()
        {
            CheckDroneCountRequest();
        }

        private void CheckDroneCountRequest()
        {
            if (_uiInputController.DroneCount > _currentDroneCount)
                SpawnDrone();
            if (_uiInputController.DroneCount < _currentDroneCount)
            {
                DespawnDrone();
            }
        }

        private void SpawnDrone()
        {
            _dronesArray[_currentDroneCount].gameObject.SetActive(true);
            _dronesArray[_currentDroneCount++].SpawnRequest();
        }

        private void DespawnDrone()
        {
            _dronesArray[--_currentDroneCount].DespawnRequest();
        }

        private Drone[] CreateDroneObjects(int count = 10)
        {
            Drone[] drones = new Drone[count];
            
            for (int i = 0; i < count; i++)
            {
                var drone = _droneFactory.Create(transform, team, resourcesParent, GetComponent<MeshRenderer>().material);
                drones[i] = drone;
            }
            
            return drones;
        }
    }
}
