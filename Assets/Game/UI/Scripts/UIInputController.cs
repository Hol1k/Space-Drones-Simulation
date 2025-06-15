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
        [SerializeField] private Slider dronesCountSlider;
        public int maxDroneCount = 5;
        public int DroneCount { get; private set; }

        [Space]
        [SerializeField] private Slider droneSpeedSlider;
        [SerializeField] private float maxDroneSpeed = 10f;
        public float DroneSpeed { get; private set; }
        
        private ResourceSpawner _resourceSpawner;
        [Space]
        [SerializeField] private TMP_InputField resourceSpawnFrequencyInputField;

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

        public void OnChangeDroneCount()
        {
            DroneCount = Convert.ToInt32(dronesCountSlider.value * maxDroneCount);
        }
    }
}
