using System;
using System.Collections.Generic;
using Data;
using UnityEngine;

public class Game : MonoBehaviour
{
    public event Action OnTakeDamage;
    public event Action OnTakePoint;
    public event Action OnClearPoints;
    public event Action NewStage;
    public int Health { get; private set; } = 3;
    public int Points { get; private set; }
    public int Stage { get; private set; }

    public List<UpgradeType> Upgrades { get; } = new List<UpgradeType>();

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
}
