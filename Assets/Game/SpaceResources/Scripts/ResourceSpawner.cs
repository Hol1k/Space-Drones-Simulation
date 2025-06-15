using System.Linq;
using Unity.AI.Navigation;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.SpaceResources.Scripts
{
    public class ResourceSpawner : MonoBehaviour
    {
        [SerializeField] private NavMeshSurface playingSurface;
        private Bounds _playingBounds;
        
        [SerializeField] private GameObject resourcePrefab;
        
        [SerializeField] private float droneColliderRadius = 0.5f;
        
        [Space]
        [Tooltip("resource per minute")]
        public int spawnFrequency = 10;
        [SerializeField] [Min(0)] private int maxResourcesCount = 100;
        private float _currentSpawnCooldown;
        
        [SerializeField] private float ySpawnOffset = -0.5f;

        private void Awake()
        {
            _playingBounds = playingSurface.navMeshData.sourceBounds;
        }

        private void Update()
        {
            GenerateResource();
        }

        private void GenerateResource()
        {
            if (spawnFrequency <= 0)
                return;
            
            if (transform.childCount >= maxResourcesCount)
                return;
            
            if (_currentSpawnCooldown > 0)
            {
                _currentSpawnCooldown -= Time.deltaTime;
                return;
            }
            
            Vector3 spawnPosition = RollSpawnPositionOnPlayingSurface();

            Instantiate(resourcePrefab, playingSurface.transform.position + spawnPosition, Random.rotation, transform);
            
            _currentSpawnCooldown = 60f / spawnFrequency;
        }

        private Vector3 RollSpawnPositionOnPlayingSurface()
        {
            Vector3 spawnPosition;

            bool isResourceInObstacle;
            do
            {
                spawnPosition = Vector3.zero;
                
                spawnPosition.x += Random.Range(_playingBounds.min.x, _playingBounds.max.x);
                spawnPosition.z += Random.Range(_playingBounds.min.z, _playingBounds.max.z);

                var colliders = Physics.OverlapSphere(spawnPosition, droneColliderRadius);
                isResourceInObstacle = colliders.Any(colliderHit => colliderHit.gameObject.CompareTag("Obstacle"));
            } while (isResourceInObstacle);
            
            spawnPosition.y += ySpawnOffset;
            return spawnPosition;
        }
    }
}
