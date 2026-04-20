using System;
using System.Collections.Generic;
using Data;
using UnityEngine;
using Random = UnityEngine.Random;

public class Game : MonoBehaviour
{
    public event Action OnTakeDamage;
    public event Action OnHealthChange;
    public event Action OnTakePoint;
    public event Action OnClearPoints;
    public event Action NewStage;
    public event Action EventStarted;
    public event Action EventStopped;
    public int Health { get; private set; } = 3;
    public int Points { get; private set; }
    public int Stage { get; private set; }

    public List<UpgradeType> Upgrades { get; } = new List<UpgradeType>();
    
    public GameEventType EventType { get; private set; }

    public void TakeDamage()
    {
        Health--;
        OnTakeDamage?.Invoke();
    }

    public void AddPoint()
    {
        Points++;
        OnTakePoint?.Invoke();
    }

    public void ClearPoints()
    {
        Points = 0;
        OnClearPoints?.Invoke();
    }

    public void CountStage()
    {
        Stage++;
        NewStage?.Invoke();
    }
    
    public void SetMaxHealth(int maxHealth)
    {
        Health = maxHealth;
        OnHealthChange?.Invoke();
    }

    public void StartEvent()
    {
        EventType = (GameEventType)Random.Range(1, 5);
        Debug.Log($"Event started: {EventType}");
        EventStarted?.Invoke();
    }

    public void StopEvent()
    {
        EventType = GameEventType.None;
        EventStopped?.Invoke();
    }
}
