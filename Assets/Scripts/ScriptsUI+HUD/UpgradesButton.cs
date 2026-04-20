using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System.Collections;

[RequireComponent(typeof(Button))]
public class UpgradesButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Звуки")]
    [SerializeField] private AudioClip hoverInSound;
    [SerializeField] private AudioClip hoverOutSound;
    [SerializeField] private AudioSource audioSource;

    [Header("Текст")]
    [SerializeField] private TMP_Text textMesh;

    [Header("Картинка с анимацией")]
    [SerializeField] private RectTransform hoverImage;           // ← Изменил на RectTransform
    [SerializeField] private float animationDuration = 0.25f;    // Длительность анимации
    [SerializeField] private float hideOffset = 80f;             // На сколько пикселей картинка будет выше в скрытом состоянии

    private Button button;
    private Vector2 originalAnchoredPosition;
    private Coroutine currentAnimation;

    private void Awake()
    {
        button = GetComponent<Button>();

        // Отключаем клик по кнопке
        button.interactable = true;
        button.onClick.RemoveAllListeners();

        if (textMesh == null)
            textMesh = GetComponentInChildren<TMP_Text>();

        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        // Подготовка картинки
        if (hoverImage != null)
        {
            originalAnchoredPosition = hoverImage.anchoredPosition;
            // Изначально прячем картинку сверху
            hoverImage.anchoredPosition = new Vector2(originalAnchoredPosition.x, originalAnchoredPosition.y + hideOffset);
            hoverImage.gameObject.SetActive(false);
        }

        DisableUnderline();
    }

    // ====================== НАВЕДЕНИЕ ======================
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (hoverInSound != null && audioSource != null)
            audioSource.PlayOneShot(hoverInSound);

        EnableUnderline();

        // Показываем и анимируем картинку
        if (hoverImage != null)
            StartAnimation(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (hoverOutSound != null && audioSource != null)
            audioSource.PlayOneShot(hoverOutSound);

        DisableUnderline();

        // Прячем с анимацией
        if (hoverImage != null)
            StartAnimation(false);
    }

    // ====================== АНИМАЦИЯ ВЫЕЗДА ======================
    private void StartAnimation(bool show)
    {
        if (hoverImage == null) return;

        if (currentAnimation != null)
            StopCoroutine(currentAnimation);

        currentAnimation = StartCoroutine(AnimateHoverImage(show));
    }

    private IEnumerator AnimateHoverImage(bool show)
    {
        if (!hoverImage.gameObject.activeSelf && show)
            hoverImage.gameObject.SetActive(true);

        float elapsed = 0f;
        Vector2 startPos = hoverImage.anchoredPosition;
        Vector2 targetPos = show 
            ? originalAnchoredPosition 
            : new Vector2(originalAnchoredPosition.x, originalAnchoredPosition.y + hideOffset);

        while (elapsed < animationDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / animationDuration;
            t = Mathf.SmoothStep(0f, 1f, t);                    // Плавная кривая

            hoverImage.anchoredPosition = Vector2.Lerp(startPos, targetPos, t);
            yield return null;
        }

        hoverImage.anchoredPosition = targetPos;

        // Выключаем объект после скрытия
        if (!show)
            hoverImage.gameObject.SetActive(false);
    }

    // ====================== ПОДЧЁРКИВАНИЕ ======================
    private void EnableUnderline()
    {
        if (textMesh != null)
            textMesh.fontStyle |= FontStyles.Underline;
    }

    private void DisableUnderline()
    {
        if (textMesh != null)
            textMesh.fontStyle &= ~FontStyles.Underline;
    }
}