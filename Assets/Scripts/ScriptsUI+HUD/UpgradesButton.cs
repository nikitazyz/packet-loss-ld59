using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

[RequireComponent(typeof(Button))]
public class UpgradesButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Звуки")]
    [SerializeField] private AudioClip hoverSound;      // Звук при наведении
    [SerializeField] private AudioClip clickSound;      // Звук при нажатии
    [SerializeField] private AudioSource audioSource;

    [Header("Текст")]
    [SerializeField] private TMP_Text textMesh;         // TextMeshPro текст кнопки

    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();

        // Автоматически находим текст, если не назначен
        if (textMesh == null)
            textMesh = GetComponentInChildren<TMP_Text>();

        // Подписываемся на нажатие кнопки
        button.onClick.AddListener(OnButtonClicked);

        // Если AudioSource не назначен — ищем на кнопке
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        // Изначально подчёркивание выключено
        DisableUnderline();
    }

    private void OnDestroy()
    {
        if (button != null)
            button.onClick.RemoveListener(OnButtonClicked);
    }

    // ====================== НАЖАТИЕ ======================
    private void OnButtonClicked()
    {
        if (clickSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(clickSound);
        }

        // Здесь можешь добавить свою логику для этой кнопки
        Debug.Log($"Кнопка нажата: {gameObject.name}");
    }

    // ====================== НАВЕДЕНИЕ ======================
    public void OnPointerEnter(PointerEventData eventData)
    {
        // Звук при наведении
        if (hoverSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(hoverSound);
        }

        // Включаем подчёркивание
        EnableUnderline();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Выключаем подчёркивание
        DisableUnderline();
    }

    // ====================== ПОДЧЁРКИВАНИЕ ======================
    private void EnableUnderline()
    {
        if (textMesh != null)
        {
            textMesh.fontStyle = textMesh.fontStyle | FontStyles.Underline;
        }
    }

    private void DisableUnderline()
    {
        if (textMesh != null)
        {
            textMesh.fontStyle = textMesh.fontStyle & ~FontStyles.Underline;
        }
    }
}