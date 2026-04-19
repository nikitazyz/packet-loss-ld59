using System;
using UnityEngine;

public class Game : MonoBehaviour
{
    public event Action OnTakeDamage;
    public event Action OnTakePoint;
    public event Action OnClearPoints;
    public int Health { get; private set; } = 3;
    public int Points { get; private set; }

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
}
