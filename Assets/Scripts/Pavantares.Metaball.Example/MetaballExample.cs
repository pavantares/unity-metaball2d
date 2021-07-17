using System;
using UnityEngine;

namespace Pavantares.Metaball.Example
{
    public class MetaballExample : MonoBehaviour
    {
        [SerializeField]
        private SimpleSlider simpleSliderPrefab;

        [Space]
        [SerializeField]
        private Transform uiRoot;

        [Space]
        [SerializeField]
        private LineRenderer line;

        [SerializeField]
        private Transform center1;

        [SerializeField]
        private Transform center2;

        private float r1;
        private float r2;
        private float handleSize;
        public float v;
        public float limit;

        private Metaball2DRenderer metaball2DRenderer;

        private void Awake()
        {
            metaball2DRenderer = new Metaball2DRenderer(line);

            CreateSimpleSlider("R1", 0.5f, 3, 1.5f, metaball2DRenderer.SetR1);
            CreateSimpleSlider("R2", 0.25f, 1, 0.75f, metaball2DRenderer.SetR2);
            CreateSimpleSlider("Handle Size", 0.5f, 2.4f, 2.4f, x => handleSize = x);
            CreateSimpleSlider("V", 0.1f, 1, 0.5f, x => v = x);
            CreateSimpleSlider("Limit", 0.5f, 8, 5, x => limit = x);
        }

        private void Update()
        {
            metaball2DRenderer.Render(center1.position, center2.position, limit, handleSize, v);
        }

        private void CreateSimpleSlider(string title, float min, float max, float value, Action<float> onValueChanged)
        {
            var slider = Instantiate(simpleSliderPrefab, uiRoot);
            slider.Initialize(title, min, max, value);
            slider.OnValueChanged += onValueChanged;

            onValueChanged?.Invoke(value);
        }
    }
}