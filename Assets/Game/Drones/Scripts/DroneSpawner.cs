using UnityEngine;
using Zenject;

namespace Game.Drones.Scripts
{
    public class DroneSpawner : MonoBehaviour
    {
        private Drone.Factory _droneFactory;
        
        [SerializeField] private GameObject dronePrefab;

        [Range(0, 10)] public int droneCountRequest;
        private int _currentDroneCount = 0;

        [SerializeField] [Range(0, 10)] private int maxDronesCount;
        private Drone[] _dronesArray;

        [SerializeField] private GameObject resourcesParent;
        
        [Inject]
        private void Construct(Drone.Factory droneFactory)
        {
            _droneFactory = droneFactory;
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
            if (droneCountRequest > _currentDroneCount)
                SpawnDrone();
            if (droneCountRequest < _currentDroneCount)
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
                var drone = _droneFactory.Create(transform, resourcesParent);
                drones[i] = drone;
            }
            
            return drones;
        }
    }
}
