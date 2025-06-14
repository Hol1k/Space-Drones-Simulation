using UnityEngine;
using Zenject;

namespace Game.Drones.Scripts
{
    public partial class Drone
    {
        public class Factory : PlaceholderFactory<Drone>
        {
            public Drone Create(Transform fractionBaseTransform, GameObject resourcesParentObject)
            {
                var drone = base.Create();
                
                drone.droneFractionBase = fractionBaseTransform;
                drone.resourcesParent = resourcesParentObject;
                drone.transform.SetParent(fractionBaseTransform);
                drone.transform.localPosition = Vector3.zero;
                
                return drone;
            }
        }
    }
}
