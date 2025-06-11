using System;
using UnityEngine;
using UnityEngine.AI;

namespace Game.Drones.Scripts
{
    public class DroneController : MonoBehaviour
    {
        [SerializeField] private GameObject resourcesParent;
        
        private NavMeshAgent _navMeshAgent;
        
        private DroneState _droneState;
        
        [SerializeField] private Transform droneFractionBase;
        
        [Space]
        [SerializeField] private float distanceForLootingOrDropping;
        private Transform _targetResource;

        [SerializeField] private float lootingOrDroppingDuration;
        private float _lootOrDropProgress;
        
        private bool _itHasResource;

        private void Awake()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
        }

        private void Start()
        {
            _droneState = DroneState.ChoosingResource;
        }

        private void Update()
        {
            ChoseBehaviour();
        }

        private void ChoseBehaviour()
        {
            switch (_droneState)
            {
                case DroneState.ChoosingResource:
                    ChoseTargetResource();
                    break;
                case DroneState.FollowingResource:
                    FollowingToResource();
                    break;
                case DroneState.LootingResource:
                    LootingResource();
                    break;
                case DroneState.FollowingBase:
                    FollowingToBase();
                    break;
                case DroneState.DroppingResource:
                    DroppingResource();
                    break;
            }
        }

        private void ChoseTargetResource()
        {
            if (resourcesParent.transform.childCount == 0)
                return;
            
            Transform closestResource = null;
            float lowestDistance = float.MaxValue;
            
            foreach (Transform resource in resourcesParent.transform)
            {
                float distance = Vector3.Distance(resource.position, transform.position);
                
                if (distance < lowestDistance)
                {
                    lowestDistance = distance;
                    closestResource = resource;
                }
            }
            
            if (!closestResource)
                return;
            
            _targetResource = closestResource;
            _droneState = DroneState.FollowingResource;
            _navMeshAgent.SetDestination(_targetResource.position);
        }

        private void FollowingToResource()
        {
            if (!_targetResource)
                _droneState = DroneState.ChoosingResource;
            
            if (_navMeshAgent.remainingDistance <= distanceForLootingOrDropping)
                _droneState = DroneState.LootingResource;
        }

        private void LootingResource()
        {
            if (!_targetResource)
            {
                _lootOrDropProgress = 0f;
                _droneState = DroneState.ChoosingResource;
            }
            
            if (_lootOrDropProgress < lootingOrDroppingDuration)
            {
                _lootOrDropProgress += Time.deltaTime;
            }
            else
            {
                _lootOrDropProgress = 0f;
                _itHasResource = true;
                Destroy(_targetResource.gameObject);
                _targetResource = null;
                
                _navMeshAgent.SetDestination(droneFractionBase.position);
                _droneState = DroneState.FollowingBase;
            }
        }

        private void FollowingToBase()
        {
            if (!droneFractionBase)
                return;
            
            if (!_itHasResource)
                _droneState = DroneState.ChoosingResource;

            if (_navMeshAgent.remainingDistance <= distanceForLootingOrDropping)
                _droneState = DroneState.DroppingResource;
        }

        private void DroppingResource()
        {
            if (!droneFractionBase.gameObject)
                return;
            
            if (!_itHasResource)
                _droneState = DroneState.ChoosingResource;
            
            if (_lootOrDropProgress < lootingOrDroppingDuration)
            {
                _lootOrDropProgress += Time.deltaTime;
            }
            else
            {
                _lootOrDropProgress = 0f;
                _itHasResource = false;
                
                _droneState = DroneState.ChoosingResource;
            }
        }
    }
}
