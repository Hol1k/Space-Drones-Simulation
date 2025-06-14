using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
using Zenject;

namespace Game.Drones.Scripts
{
    public partial class Drone : MonoBehaviour
    {
        public GameObject resourcesParent;
        
        private NavMeshAgent _navMeshAgent;
        
        private DroneState _droneState;
        
        public Transform droneFractionBase;
        
        public MeshRenderer droneMeshRenderer;
        
        [Space]
        [SerializeField] private float distanceForLootingOrDropping;
        private Transform _targetResource;
        private List<Transform> _occupiedResources;

        [SerializeField] private float lootingOrDroppingDuration;
        private float _lootOrDropProgress;
        
        private bool _itHasResource;
        
        private bool _isDroneActive;
        
        [Space]
        [SerializeField] private Animator animator;
        private bool _despawnRequested = true;
        private static readonly int SpawnTriggerString = Animator.StringToHash("SpawnTrigger");
        private static readonly int DespawnTriggerString = Animator.StringToHash("DespawnTrigger");
        [Tooltip("seconds")]
        [SerializeField] private float spawnOrDespawnDuration = 0.5f;

        [Inject]
        private void Construct(List<Transform> occupiedResources)
        {
            _occupiedResources = occupiedResources;
        }

        private void Awake()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
        }

        private void Start()
        {
            _droneState = DroneState.Despawning;
            if (_occupiedResources == null)
            {
                Debug.LogError("_occupiedResources is NULL");
            }
        }

        private void Update()
        {
            ChoseBehaviour();
        }

        public void SpawnRequest()
        {
            if (!_isDroneActive)
            {
                _droneState = DroneState.Spawning;
            }
            else
            {
                _despawnRequested = false;
                _droneState = _itHasResource ? DroneState.FollowingBase : DroneState.ChoosingResource;
            }
        }

        public void DespawnRequest()
        {
            _despawnRequested = true;
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
                case DroneState.Spawning:
                    Spawn();
                    break;
                case DroneState.Despawning:
                    Despawn();
                    break;
            }
        }

        private void ChoseTargetResource()
        {
            if (_despawnRequested)
            {
                _occupiedResources.Remove(_targetResource);
                _targetResource = null;
                _navMeshAgent.SetDestination(droneFractionBase.position);
                _droneState = DroneState.FollowingBase;
            }
            
            if (resourcesParent.transform.childCount == 0)
                return;
            
            Transform closestResource = null;
            float lowestDistance = float.MaxValue;
            
            foreach (Transform resource in resourcesParent.transform)
            {
                if (_occupiedResources.Contains(resource))
                    continue;
                
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
            _occupiedResources.Add(_targetResource);
            
            _droneState = DroneState.FollowingResource;
            _navMeshAgent.SetDestination(_targetResource.position);
        }

        private void FollowingToResource()
        {
            if (_despawnRequested)
            {
                _occupiedResources.Remove(_targetResource);
                _targetResource = null;
                _navMeshAgent.SetDestination(droneFractionBase.position);
                _droneState = DroneState.FollowingBase;
            }
            else if (!_targetResource)
            {
                _droneState = DroneState.ChoosingResource;
            }
            
            if (_navMeshAgent.remainingDistance <= distanceForLootingOrDropping)
                _droneState = DroneState.LootingResource;
        }

        private void LootingResource()
        {
            if (_despawnRequested)
            {
                _occupiedResources.Remove(_targetResource);
                _targetResource = null;
                _navMeshAgent.SetDestination(droneFractionBase.position);
                _droneState = DroneState.FollowingBase;
                _lootOrDropProgress = 0f;
            }
            else if (!_targetResource)
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
                _occupiedResources.Remove(_targetResource);
                if (_targetResource) Destroy(_targetResource.gameObject);
                _targetResource = null;
                
                _navMeshAgent.SetDestination(droneFractionBase.position);
                _droneState = DroneState.FollowingBase;
            }
        }

        private void FollowingToBase()
        {
            if (!droneFractionBase)
                return;

            if (_navMeshAgent.remainingDistance <= distanceForLootingOrDropping)
            {
                if (_itHasResource)
                    _droneState = DroneState.DroppingResource;
                else if (_despawnRequested)
                    _droneState = DroneState.Despawning;
                else
                    _droneState = DroneState.ChoosingResource;
            }
        }

        private void DroppingResource()
        {
            if (!droneFractionBase.gameObject)
                return;
            
            if (_lootOrDropProgress < lootingOrDroppingDuration)
            {
                _lootOrDropProgress += Time.deltaTime;
            }
            else
            {
                _lootOrDropProgress = 0f;
                _itHasResource = false;
                
                _droneState = _despawnRequested ? DroneState.Despawning : DroneState.ChoosingResource;
            }
        }

        private void Spawn()
        {
            animator.SetTrigger(SpawnTriggerString);
            _isDroneActive = true;
            _droneState = DroneState.ChoosingResource;
            _navMeshAgent.enabled = true;
        }

        private void Despawn()
        {
            if (_despawnRequested)
            {
                _despawnRequested = false;
                StartCoroutine(DespawnCoroutine());
            }
        }

        private IEnumerator DespawnCoroutine()
        {
            animator.SetTrigger(DespawnTriggerString);
            yield return new WaitForSeconds(spawnOrDespawnDuration);
            _isDroneActive = false;
            gameObject.SetActive(false);
        }
    }
}
