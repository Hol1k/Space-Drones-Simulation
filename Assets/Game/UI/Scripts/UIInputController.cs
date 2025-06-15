using System;
using Game.SpaceResources.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Game.UI.Scripts
{
    public class UIInputController : MonoBehaviour
    {
        private ResourceSpawner _resourceSpawner;
        [SerializeField] private TMP_InputField resourceSpawnFrequencyInputField;

        [Space]
        [SerializeField] private Slider droneSpeedSlider;
        [SerializeField] private float maxDroneSpeed = 10f;
        public float DroneSpeed { get; private set; }

        [Inject]
        private void Construct(ResourceSpawner resourceSpawner)
        {
            _resourceSpawner = resourceSpawner;
        }

        public void OnChangeResourceSpawnFrequency()
        {
            if (string.IsNullOrWhiteSpace(resourceSpawnFrequencyInputField.text))
                _resourceSpawner.spawnFrequency = 0;
                
            _resourceSpawner.spawnFrequency = Convert.ToInt32(resourceSpawnFrequencyInputField.text);
        }

        public void OnChangeDroneSpeed()
        {
            DroneSpeed = droneSpeedSlider.value * maxDroneSpeed;
        }
    }
}
