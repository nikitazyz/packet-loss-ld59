using System;
using System.Collections.Generic;
using Data;
using PrimeTween;
using TMPro;
using UnityEngine;

namespace UI
{
    public class GameEventPanel : MonoBehaviour
    {
        [SerializeField] private GameObject _eventPanel;
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private Game _game;

        [SerializeField] private AudioClip _eventSound;
        [SerializeField] private AudioSource _audioSource;

        private Dictionary<GameEventType, string> _events = new()
        {
            { GameEventType.Buffer, "Memory Leak!\nTime in buffer decreased!" },
            { GameEventType.Sending, "Connection time-out!\nSending time increased!" },
            { GameEventType.Deleting, "Permission denied!\nDeleting time increased!" },
            { GameEventType.Verifying, "Stack Overflow!\nVerifying time increased!" }
        };

        private void Awake()
        {
            _game.EventStarted += GameOnEventStarted;
            _game.EventStopped += () => _eventPanel.SetActive(false);
        }

        private void GameOnEventStarted()
        {
            _eventPanel.SetActive(true);
            _text.text = _events[_game.EventType];
            _audioSource.PlayOneShot(_eventSound);
            _eventPanel.transform.localScale = Vector3.zero;
            Tween.Scale(_eventPanel.transform, new TweenSettings<float>(1, 0.5f, Ease.OutElastic));
        }
        
        
    }
}