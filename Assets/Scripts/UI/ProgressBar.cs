using System;
using UnityEngine;

namespace UI
{
    public class ProgressBar : MonoBehaviour
    {
        [SerializeField] private RectTransform _fill;
        [SerializeField, Range(0f,1f)] private float _value;

        public float Value
        {
            get => _value;
            set
            {
                _value = Mathf.Clamp01(value);
                UpdateUI();
            }
        }

        private RectTransform _bar;

        private void Awake()
        {
            _bar = GetComponent<RectTransform>();
        }

        private void UpdateUI()
        {
            _fill.pivot = new Vector2(0, 1);
            _fill.anchorMin = new Vector2(0, 1);
            _fill.anchorMax = new Vector2(1, 1);
            _fill.offsetMin = new Vector2(0, 0);
            _fill.offsetMax = new Vector2(0, 0);
            
            _fill.sizeDelta = new Vector2(-Mathf.Lerp(0, _bar.rect.width, 1-Mathf.Clamp01(_value)),  _bar.rect.height);
        }
    }
}
