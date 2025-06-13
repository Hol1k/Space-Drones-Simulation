using UnityEngine;

namespace Game.Drones.Scripts
{
    public class DroneSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject dronePrefab;
        
        [Range(0, 10)] public int droneCountRequest;
        private int _currentDroneCount = 0;

        [SerializeField] [Range(0, 10)] private int maxDronesCount;
        private DroneController[] _dronesArray;

        [SerializeField] private GameObject resourcesParent;

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

        private DroneController[] CreateDroneObjects(int count = 10)
        {
            DroneController[] drones = new DroneController[count];
            
            for (int i = 0; i < count; i++)
            {
                var DroneObject = Instantiate(dronePrefab, transform);
                var drone = DroneObject.GetComponent<DroneController>();
                drone.droneFractionBase = transform;
                drone.resourcesParent = resourcesParent;
                drones[i] = drone;
            }
            
            return drones;
        }
    }
}
