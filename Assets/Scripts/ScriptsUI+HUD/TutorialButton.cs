using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using PrimeTween;

[RequireComponent(typeof(Button))]
public class TutorialButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Звуки")]
    [SerializeField] private AudioClip hoverSound;      // Звук при наведении
    [SerializeField] private AudioClip clickSound;      // Звук при нажатии
    [SerializeField] private AudioSource audioSource;

    [Header("Визуальный эффект")]
    [SerializeField] private float hoverScale = 1.12f;   // Размер при наведении
    [SerializeField] private float pressScale = 0.90f;   // Размер при нажатии (уменьшение)

    [Header("Меню туториала")]
    [SerializeField] private GameObject tutorialPanel;   // Панель с подсказкой, которая должна открыться

    private Button button;
    private RectTransform rectTransform;
    private Vector3 originalScale;

    private void Awake()
    {
        button = GetComponent<Button>();
        rectTransform = GetComponent<RectTransform>();
        originalScale = rectTransform.localScale;

        button.onClick.AddListener(OnTutorialClicked);

        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
    }

    private void OnDestroy()
    {
        button.onClick.RemoveListener(OnTutorialClicked);
    }

    // ====================== НАЖАТИЕ ======================
    private void OnTutorialClicked()
    {
        // Звук нажатия
        if (clickSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(clickSound);
        }

        // Эффект нажатия — кнопка становится меньше
        Tween.Scale(rectTransform, new TweenSettings<Vector3>(originalScale * pressScale, 0.08f));

        // Открываем меню туториала
        if (tutorialPanel != null)
        {
            tutorialPanel.SetActive(true);
        }
    }

    // ====================== НАВЕДЕНИЕ ======================
    public void OnPointerEnter(PointerEventData eventData)
    {
        // Звук при наведении
        if (hoverSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(hoverSound);
        }

        // Увеличиваем кнопку
        Tween.Scale(rectTransform, new TweenSettings<Vector3>(originalScale * hoverScale, 0.15f));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Возвращаем кнопку к обычному размеру
        Tween.Scale(rectTransform, new TweenSettings<Vector3>(originalScale, 0.15f));
    }
}