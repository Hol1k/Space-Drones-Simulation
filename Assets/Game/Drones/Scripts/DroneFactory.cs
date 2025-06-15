using UnityEngine;
using Zenject;

namespace Game.Drones.Scripts
{
    public partial class Drone
    {
        public class Factory : PlaceholderFactory<Drone>
        {
            public Drone Create(Transform fractionBaseTransform, DroneTeam team, GameObject resourcesParentObject, Material droneMaterial)
            {
                var drone = base.Create();
                
                drone.droneFractionBase = fractionBaseTransform;
                drone.team = team;
                drone.resourcesParent = resourcesParentObject;
                drone.transform.SetParent(fractionBaseTransform);
                drone.transform.localPosition = Vector3.zero;
                drone.droneMeshRenderer.material = droneMaterial;
                
                return drone;
            }
        }
    }
}
