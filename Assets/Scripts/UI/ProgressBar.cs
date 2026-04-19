using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ProgressBar : MonoBehaviour
    {
        [SerializeField] private Image _fill;
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

        private void UpdateUI()
        {
            _fill.fillAmount = _value;
        }
    }
}
