using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonSound : MonoBehaviour
{
    [Header("Звук при нажатии")]
    [SerializeField] private AudioClip clickSound;

    [Header("Настройки")]
    [SerializeField] private AudioSource audioSource;     // Можно оставить пустым

    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();

        // Если AudioSource не назначен — ищем на этом же объекте
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        // Подписываемся на нажатие кнопки
        button.onClick.AddListener(PlaySound);
    }

    private void OnDestroy()
    {
        if (button != null)
            button.onClick.RemoveListener(PlaySound);
    }

    private void PlaySound()
    {
        if (clickSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(clickSound);
        }
    }
}