using System;
using TMPro;
using UnityEngine;

namespace UI
{
    public class Points : MonoBehaviour
    {
        [SerializeField] private Game _game;
        [SerializeField] private TrafficManager _traffic;
        [SerializeField] private TextMeshProUGUI _text;

        private void Awake()
        {
            _game.OnTakePoint += OnTakePoint;
            _game.OnClearPoints += OnTakePoint;
        }

        private void OnTakePoint()
        {
            _text.text = $"{_game.Points}/{_traffic.CurrentTraffic.PointsToComplete} to complete the stage";
        }
    }
}