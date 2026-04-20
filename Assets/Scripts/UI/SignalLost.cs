using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class SignalLost : MonoBehaviour
{
    [SerializeField] private Image _image;

    [SerializeField] private Sprite _good;
    [SerializeField] private Sprite _medium;
    [SerializeField] private Sprite _bad;

    [SerializeField] private Game _game;

    private bool _isBadSignal;

    private void Awake()
    {
        _game.EventStarted += GameOnEventStarted;
        _game.EventStopped += GameOnEventStopped;
    }

    private void GameOnEventStopped()
    {
        _isBadSignal = false;
        _image.sprite = _good;
    }

    private void Start()
    {
        StartCoroutine(SignalAnimation());
    }

    private void GameOnEventStarted()
    {
        _isBadSignal = true;
    }
    
    

    IEnumerator SignalAnimation()
    {
        while (true)
        {
            if (_isBadSignal)
            {
                _image.sprite = Random.value > 0.5f ? _bad : _medium;
            }
            yield return new WaitForSeconds(Random.Range(1f, 2f));
        }
    }
}
