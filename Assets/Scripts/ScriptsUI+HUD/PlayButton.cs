using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System.Collections;
using PrimeTween;

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
    [SerializeField] private Image _fadeImage;

    private Button button;
    private RectTransform rectTransform;

    private void Awake()
    {
        button = GetComponent<Button>();
        rectTransform = GetComponent<RectTransform>();

        // Подписка на нажатие кнопки
        button.onClick.AddListener(OnPlayClicked);

        // Если AudioSource не назначен — ищем на этом объекте
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        _fadeImage.color = new Color(0,0,0,1);
        _fadeImage.gameObject.SetActive(true);
    }

    private void Start()
    {
        Tween.Alpha(_fadeImage, new TweenSettings<float>(0, 0.15f))
            .OnComplete(() => _fadeImage.gameObject.SetActive(false));
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

        _fadeImage.gameObject.SetActive(true);
        Tween.Alpha(_fadeImage, new TweenSettings<float>(1, 0.15f))
            .OnComplete(() => SceneManager.LoadScene(sceneToLoad));
        
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
        Tween.Scale(transform, new TweenSettings<float>(hoverScale, 0.15f));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Плавно возвращаем кнопку в исходный размер
        StopAllCoroutines();
        Tween.Scale(transform, new TweenSettings<float>(1, 0.15f));
    }
}