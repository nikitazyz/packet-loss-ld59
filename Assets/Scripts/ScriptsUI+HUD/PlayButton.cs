using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using PrimeTween;

[RequireComponent(typeof(Button))]
public class PlayButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Переход на сцену")]
    [SerializeField] private string sceneToLoad = "Game";

    [Header("Звуки")]
    [SerializeField] private AudioClip hoverSound;
    [SerializeField] private AudioClip clickSound;
    [SerializeField] private AudioSource audioSource;

    [Header("Визуальный эффект")]
    [SerializeField] private float hoverScale = 1.12f;      // Размер при наведении
    [SerializeField] private float pressScale = 0.92f;      // Размер при нажатии (меньше)
    [SerializeField] private Image _fadeImage;

    private Button button;
    private RectTransform rectTransform;
    private Vector3 originalScale;

    private void Awake()
    {
        button = GetComponent<Button>();
        rectTransform = GetComponent<RectTransform>();
        originalScale = rectTransform.localScale;           // Запоминаем исходный размер

        button.onClick.AddListener(OnPlayClicked);

        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        // Настройка fade изображения
        if (_fadeImage != null)
        {
            _fadeImage.color = new Color(0, 0, 0, 1);
            _fadeImage.gameObject.SetActive(true);
        }
    }

    private void Start()
    {
        if (_fadeImage != null)
        {
            Tween.Alpha(_fadeImage, new TweenSettings<float>(0, 0.15f))
                .OnComplete(() => _fadeImage.gameObject.SetActive(false));
        }
    }

    private void OnDestroy()
    {
        button.onClick.RemoveListener(OnPlayClicked);
    }

    // ====================== НАЖАТИЕ ======================
    private void OnPlayClicked()
    {
        if (clickSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(clickSound);
        }

        // Эффект нажатия — кнопка становится меньше
        Tween.Scale(rectTransform, new TweenSettings<float>(pressScale, 0.08f));

        // Fade + переход на сцену
        if (_fadeImage != null)
        {
            _fadeImage.gameObject.SetActive(true);
            Tween.Alpha(_fadeImage, new TweenSettings<float>(1, 0.15f))
                .OnComplete(() => SceneManager.LoadScene(sceneToLoad));
        }
        else
        {
            SceneManager.LoadScene(sceneToLoad);
        }
    }

    // ====================== НАВЕДЕНИЕ ======================
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (hoverSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(hoverSound);
        }

        // При наведении — увеличиваем кнопку
        Tween.Scale(rectTransform, new TweenSettings<float>(hoverScale, 0.15f));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // При уходе мыши — возвращаем к нормальному размеру
        Tween.Scale(rectTransform, new TweenSettings<float>(1f, 0.15f));
    }
}