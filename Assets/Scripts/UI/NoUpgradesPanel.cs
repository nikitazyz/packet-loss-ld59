using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class NoUpgradesPanel : MonoBehaviour
    {
        [SerializeField] private GameObject _panel;
        [SerializeField] private Button _button;

        private Action _callback;
        private void Awake()
        {
            _panel.SetActive(false);
            _button.onClick.AddListener(OnButton);
        }

        private void OnButton()
        {
            _panel.SetActive(false);
            _callback?.Invoke();
        }

        public void ShowPanel(Action callback)
        {
            _callback = callback;
            _panel.SetActive(true);
        }
    }
}