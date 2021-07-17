using System;
using UnityEngine;
using UnityEngine.UI;

namespace Pavantares.Metaball.Example
{
    public class SimpleSlider : MonoBehaviour
    {
        [SerializeField]
        private Slider slider;

        [SerializeField]
        private Text valueText;

        [SerializeField]
        private Text titleText;

        public event Action<float> OnValueChanged;

        private void OnEnable()
        {
            slider.onValueChanged.AddListener(HandleValueChanged);
        }

        private void OnDisable()
        {
            slider.onValueChanged.RemoveListener(HandleValueChanged);
        }

        public void Initialize(string title, float min, float max, float value)
        {
            titleText.text = title;
            slider.minValue = min;
            slider.maxValue = max;
            slider.value = value;
            valueText.text = value.ToString();
        }

        private void HandleValueChanged(float value)
        {
            valueText.text = $"{value:0.00}";
            OnValueChanged?.Invoke(value);
        }
    }
}