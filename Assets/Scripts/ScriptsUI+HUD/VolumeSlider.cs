using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class VolumeSlider : MonoBehaviour
{
    [Header("Настройки слайдера громкости")]
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private string volumeParameter = "MasterVolume";

    [SerializeField] private Slider slider;

    private bool isLoading = false;

    private void Awake()
    {
        if (slider == null)
            slider = GetComponent<Slider>();

        if (slider != null)
            slider.onValueChanged.AddListener(OnSliderValueChanged);

        slider.minValue = 0.0001f;
        slider.maxValue = 1f;
        audioMixer.GetFloat(volumeParameter, out var currentVolume);
        slider.value = Mathf.Pow(10, currentVolume / 20f);
    }

    private void OnDestroy()
    {
        if (slider != null)
            slider.onValueChanged.RemoveListener(OnSliderValueChanged);
    }

    private void OnSliderValueChanged(float sliderValue)
    {
        if (isLoading) return;

        if (audioMixer != null)
        {
            float converted = Mathf.Log10(sliderValue) * 20;
            audioMixer.SetFloat(volumeParameter, converted);
        }
    }
}