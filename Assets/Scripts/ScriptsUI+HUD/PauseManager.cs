using System;
using PrimeTween;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{

    [SerializeField] private string _menuScene;
    [SerializeField] private Game _game;
    
    [Header("Панели")]
    [SerializeField] private GameObject pausePanel;        // Панель паузы
    [SerializeField] private GameObject confirmPanel;      // Окно подтверждения "Выйти в меню?"
    [SerializeField] private GameObject gameOverPanel;     // ← Новая панель Game Over
    [SerializeField] private GameObject _tutorialPanel;
    [SerializeField] private Image _fadeImage;

    [Header("Кнопка паузы в игре")]
    [SerializeField] private GameObject pauseButton;       // Кнопка паузы (в HUD)

    private void Awake()
    {
        _game.OnTakeDamage += CheckGameOver;
        _fadeImage.gameObject.SetActive(true);
        _fadeImage.color = new Color(0, 0, 0, 1f);
    }

    private void CheckGameOver()
    {
        if (_game.Health <= 0)
        {
            ShowGameOver();
        }
    }

    private void Start()
    {
        pausePanel.SetActive(false);
        confirmPanel.SetActive(false);
        gameOverPanel.SetActive(false);

        Tween.Alpha(_fadeImage, new TweenSettings<float>(0, 0.15f))
            .OnComplete(() => _fadeImage.gameObject.SetActive(false));
    }

    // ====================== ПАУЗА ======================
    public void PauseGame()
    {
        Time.timeScale = 0f;

        if (pauseButton != null) 
            pauseButton.SetActive(false);

        if (pausePanel != null) 
            pausePanel.SetActive(true);

        if (confirmPanel != null) 
            confirmPanel.SetActive(false);

        if (gameOverPanel != null) 
            gameOverPanel.SetActive(false);   // на всякий случай
    }

    // ====================== ПРОДОЛЖИТЬ ИГРУ ======================
    public void ResumeGame()
    {
        Time.timeScale = 1f;

        if (pausePanel != null) 
            pausePanel.SetActive(false);

        if (confirmPanel != null) 
            confirmPanel.SetActive(false);

        if (pauseButton != null) 
            pauseButton.SetActive(true);
    }

    // ====================== ОТКРЫТЬ ОКНО ПОДТВЕРЖДЕНИЯ ======================
    public void OpenConfirmPanel()
    {
        if (confirmPanel != null) 
            confirmPanel.SetActive(true);
        
    }

    // ====================== ПОДТВЕРЖДЕНИЕ ВЫХОДА В МЕНЮ ======================
    public void ConfirmGoToMenu()
    {
        Time.timeScale = 1f;
        _fadeImage.gameObject.SetActive(true);
        Tween.Alpha(_fadeImage, new TweenSettings<float>(1, 0.15f))
            .OnComplete(() => SceneManager.LoadScene(_menuScene));
    }

    // ====================== ОТМЕНА ======================
    public void CancelGoToMenu()
    {
        if (confirmPanel != null) 
            confirmPanel.SetActive(false);
    }

    // ====================== GAME OVER ======================
    public void ShowGameOver()
    {
        Time.timeScale = 0f;

        if (pauseButton != null) 
            pauseButton.SetActive(false);

        if (pausePanel != null) 
            pausePanel.SetActive(false);

        if (confirmPanel != null) 
            confirmPanel.SetActive(false);

        if (gameOverPanel != null) 
            gameOverPanel.SetActive(true);
    }

    // ====================== ПЕРЕЗАПУСК УРОВНЯ ======================
    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);   // Перезапускает текущую сцену
        
    }

    public void OpenTutorialPanel()
    {
        Time.timeScale = 0;
        
        _tutorialPanel.SetActive(true);
    }

    public void CloseTutorialPanel()
    {
        Time.timeScale = 1f;
        
        _tutorialPanel.SetActive(false);
    }
}