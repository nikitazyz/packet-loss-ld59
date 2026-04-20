using System;
using UnityEngine;

public class Cat : MonoBehaviour
{
    private static readonly int Shock = Animator.StringToHash("Shock");
    private static readonly int Happy = Animator.StringToHash("Happy");
    [SerializeField] private Animator _animator;
    [SerializeField] private AudioSource _audioSource;
    
    [Header("Sounds")]
    [SerializeField] private AudioClip _errorSound;
    [SerializeField] private AudioClip _successSound;

    [SerializeField] private Game _game;

    private void Awake()
    {
        _game.OnTakeDamage += OnTakeDamage;
        _game.OnTakePoint += OnTakePoint;
    }

    private void OnTakePoint()
    {
        _animator.SetTrigger(Happy);
        _audioSource.PlayOneShot(_successSound);
    }

    private void OnTakeDamage()
    {
        _animator.SetTrigger(Shock);
        _audioSource.PlayOneShot(_errorSound);
    }
    
    
}
