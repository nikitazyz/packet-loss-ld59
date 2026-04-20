using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    [Header("Панели")]
    [SerializeField] private GameObject pausePanel;        // Панель паузы
    [SerializeField] private GameObject confirmPanel;      // Окно подтверждения "Выйти в меню?"
    [SerializeField] private GameObject gameOverPanel;     // ← Новая панель Game Over

    [Header("Кнопка паузы в игре")]
    [SerializeField] private GameObject pauseButton;       // Кнопка паузы (в HUD)

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
        SceneManager.LoadScene("MainMenu");     // ← Замени на название своей сцены меню
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

    // ====================== ВЫХОД ИЗ ИГРЫ ======================
    public void QuitGame()
    {
        Time.timeScale = 1f;

        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }
}