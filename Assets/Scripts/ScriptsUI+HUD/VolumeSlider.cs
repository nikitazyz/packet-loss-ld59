using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class VolumeSlider : MonoBehaviour
{
    [Header("Настройки слайдера громкости")]
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private string volumeParameter = "MasterVolume";

    [SerializeField] private Slider slider;

    [SerializeField] private float minVolumeDB = -80f;
    [SerializeField] private float maxVolumeDB = 0f;

    private const string VolumePrefKey = "MasterVolume";

    private bool isLoading = false;   // ← Защита от лишних вызовов

    private void Awake()
    {
        if (slider == null)
            slider = GetComponent<Slider>();

        // Подписываемся на изменение
        if (slider != null)
            slider.onValueChanged.AddListener(OnSliderValueChanged);
    }

    private void Start()
    {
        LoadVolume();   // Загружаем сохранённое значение
    }

    private void OnDestroy()
    {
        if (slider != null)
            slider.onValueChanged.RemoveListener(OnSliderValueChanged);
    }

    // Вызывается при любом изменении слайдера пользователем
    private void OnSliderValueChanged(float sliderValue)
    {
        if (isLoading) return;   // Игнорируем вызов во время загрузки

        if (audioMixer != null)
        {
            float volumeDB = Mathf.Lerp(minVolumeDB, maxVolumeDB, sliderValue);
            audioMixer.SetFloat(volumeParameter, volumeDB);
        }

        // Сохраняем
        PlayerPrefs.SetFloat(VolumePrefKey, sliderValue);
        PlayerPrefs.Save();
    }

    private void LoadVolume()
    {
        if (slider == null) return;

        isLoading = true;

        // Загружаем значение (по умолчанию 75%)
        float savedValue = PlayerPrefs.GetFloat(VolumePrefKey, 0.75f);

        // Устанавливаем значение слайдера
        slider.value = savedValue;

        // Применяем громкость в микшер
        if (audioMixer != null)
        {
            float volumeDB = Mathf.Lerp(minVolumeDB, maxVolumeDB, savedValue);
            audioMixer.SetFloat(volumeParameter, volumeDB);
        }

        isLoading = false;
    }
}