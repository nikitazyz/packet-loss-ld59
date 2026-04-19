using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private Image[] _hearts;
        [SerializeField] private Game _game;

        [SerializeField] private GameResourcePack _resourcePack;

        private void Awake()
        {
            _game.OnTakeDamage += UpdateUI;
        }

        private void UpdateUI()
        {
            for (int i = 0; i < _hearts.Length; i++)
            {
                _hearts[i].sprite = i < _game.Health ? _resourcePack.Heart : _resourcePack.EmptyHeart;
                _hearts[i].color = i < _game.Health ? Color.white : new Color(1, 1, 1, 0.5f);
            }
        }
    }
}