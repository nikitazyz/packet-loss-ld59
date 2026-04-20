using System;
using Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UpgradePanel : MonoBehaviour
    {
        [SerializeField] private Button _upgradeButton1;
        [SerializeField] private Button _upgradeButton2;
        
        [SerializeField] private TextMeshProUGUI _upgradeText1;
        [SerializeField] private TextMeshProUGUI _upgradeText2;

        [SerializeField] private Game _game;


        private FlagUpgrade _flagUpgrade1;
        private FlagUpgrade _flagUpgrade2;
        private Action _callback;

        private void Awake()
        {
            _upgradeButton1.onClick.AddListener(OnButton1);
            _upgradeButton2.onClick.AddListener(OnButton2);
            gameObject.SetActive(false);
        }

        private void OnButton1()
        {
            _flagUpgrade1.Execute(_game);
            gameObject.SetActive(false);
            _callback?.Invoke();
        }

        private void OnButton2()
        {
            _flagUpgrade2.Execute(_game);
            gameObject.SetActive(false);
            _callback?.Invoke();
        }

        public void ShowUpgradePanel(FlagUpgrade upgrade1, FlagUpgrade upgrade2, Action callback = null)
        {
            _flagUpgrade1 = upgrade1;
            _flagUpgrade2 = upgrade2;
            _callback = callback;

            _upgradeText1.text = _flagUpgrade1.Name;
            _upgradeText2.text = _flagUpgrade2.Name;
            
            gameObject.SetActive(true);
        }
    }
}