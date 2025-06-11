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
        
        [Space]
        [Tooltip("resource per minute")]
        [SerializeField] private float spawnFrequency;
        [SerializeField] [Min(0)] private int maxResourcesCount;
        private float _currentSpawnCooldown;
        
        [SerializeField] private float ySpawnOffset;

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
            
            Vector3 spawnPosition = playingSurface.transform.position;
            spawnPosition.x += Random.Range(_playingBounds.min.x, _playingBounds.max.x);
            spawnPosition.z += Random.Range(_playingBounds.min.z, _playingBounds.max.z);
            spawnPosition.y += ySpawnOffset;
            
            Instantiate(resourcePrefab, spawnPosition, Random.rotation, transform);
            
            _currentSpawnCooldown = 60f / spawnFrequency;
        }
    }
}
