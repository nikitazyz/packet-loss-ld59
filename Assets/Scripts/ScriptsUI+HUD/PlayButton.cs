using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System.Collections;

[RequireComponent(typeof(Button))]
public class PlayButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Переход на сцену")]
    [SerializeField] private string sceneToLoad = "Game";           // Название игровой сцены

    [Header("Звуки")]
    [SerializeField] private AudioClip hoverSound;                  // Звук при наведении
    [SerializeField] private AudioClip clickSound;                  // Звук при нажатии
    [SerializeField] private AudioSource audioSource;

    [Header("Визуальный эффект")]
    [SerializeField] private float hoverScale = 1.12f;              // Насколько увеличивается кнопка
    [SerializeField] private float animationSpeed = 8f;             // Скорость анимации

    private Button button;
    private RectTransform rectTransform;
    private Vector3 originalScale;

    private void Awake()
    {
        button = GetComponent<Button>();
        rectTransform = GetComponent<RectTransform>();
        originalScale = rectTransform.localScale;

        // Подписка на нажатие кнопки
        button.onClick.AddListener(OnPlayClicked);

        // Если AudioSource не назначен — ищем на этом объекте
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
    }

    private void OnDestroy()
    {
        button.onClick.RemoveListener(OnPlayClicked);
    }

    // ====================== НАЖАТИЕ ======================
    private void OnPlayClicked()
    {
        // Воспроизводим звук нажатия
        if (clickSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(clickSound);
        }

        // Переход на сцену с небольшой задержкой
        StartCoroutine(LoadSceneAfterDelay(0.15f));
    }

    private IEnumerator LoadSceneAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(sceneToLoad);
    }

    // ====================== НАВЕДЕНИЕ ======================
    public void OnPointerEnter(PointerEventData eventData)
    {
        // Воспроизводим звук наведения
        if (hoverSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(hoverSound);
        }

        // Плавно увеличиваем кнопку
        StopAllCoroutines();
        StartCoroutine(AnimateScale(true));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Плавно возвращаем кнопку в исходный размер
        StopAllCoroutines();
        StartCoroutine(AnimateScale(false));
    }

    // Плавная анимация масштаба
    private IEnumerator AnimateScale(bool isHovering)
    {
        Vector3 targetScale = isHovering ? originalScale * hoverScale : originalScale;

        while (Vector3.Distance(rectTransform.localScale, targetScale) > 0.001f)
        {
            rectTransform.localScale = Vector3.Lerp(
                rectTransform.localScale, 
                targetScale, 
                Time.deltaTime * animationSpeed
            );
            yield return null;
        }

        rectTransform.localScale = targetScale;
    }
}