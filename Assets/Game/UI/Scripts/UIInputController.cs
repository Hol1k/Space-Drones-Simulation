using System;
using Game.SpaceResources.Scripts;
using TMPro;
using UnityEngine;
using Zenject;

namespace Game.UI.Scripts
{
    public class UIInputController : MonoBehaviour
    {
        private ResourceSpawner _resourceSpawner;
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
    }
}
