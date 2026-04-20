using System;
using System.Collections.Generic;
using Data;
using UnityEngine;

namespace UI
{
    public class UpgradesList : MonoBehaviour
    {
        [SerializeField] private UpgradesListEntry _template;
        [SerializeField] private FlagUpgrade[] _upgrades;
        [SerializeField] private Game _game;

        private readonly Dictionary<UpgradeType, UpgradesListEntry> _upgradesList = new();
    
        private void Awake()
        {
            foreach (var upgrade in _upgrades)
            {
                var instance = Instantiate(_template, transform);
                instance.Text.text = upgrade.Name;
                instance.gameObject.SetActive(true);

                _upgradesList[upgrade.UpgradeType] = instance;
                
                var imgColor = instance.Image.color;
                var txtColor = instance.Text.color;
                
                imgColor.a = 0.5f;
                instance.Image.color = imgColor;
                
                txtColor.a = 0.5f;
                instance.Text.color = txtColor;
            }
        }

        private void OnEnable()
        {
            foreach (var gameUpgrade in _game.Upgrades)
            {
                var upgrade = _upgradesList[gameUpgrade];
                
                var imgColor = upgrade.Image.color;
                var txtColor = upgrade.Text.color;
                
                imgColor.a = 1;
                upgrade.Image.color = imgColor;
                
                txtColor.a = 1;
                upgrade.Text.color = txtColor;
            }
        }
    }
}
