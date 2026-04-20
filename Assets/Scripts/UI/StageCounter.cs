using System;
using TMPro;
using UnityEngine;

public class StageCounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _stageText;
    [SerializeField] private Game _game;

    private void Awake()
    {
        _game.NewStage += GameOnNewStage;
    }

    private void GameOnNewStage()
    {
        _stageText.text = $"Stage: {_game.Stage}";
    }
}
