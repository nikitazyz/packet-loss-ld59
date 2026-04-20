using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using PrimeTween;

[RequireComponent(typeof(Button))]
public class ButtonVisual : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    [Header("Настройки эффектов")]
    [SerializeField] private bool enableHover = true;
    [SerializeField] private bool enablePress = true;

    [Header("Значения масштаба")]
    [SerializeField] private float hoverScale = 1.12f;
    [SerializeField] private float pressScale = 0.92f;

    [Header("Скорость анимации")]
    [SerializeField] private float hoverDuration = 0.15f;
    [SerializeField] private float pressDuration = 0.08f;

    private RectTransform rectTransform;
    private Vector3 originalScale;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        originalScale = rectTransform.localScale;

        // Отключаем надоедливое предупреждение PrimeTween
        PrimeTweenConfig.warnEndValueEqualsCurrent = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (enableHover)
            Tween.Scale(rectTransform, originalScale * hoverScale, hoverDuration, Ease.OutQuad);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (enableHover)
            Tween.Scale(rectTransform, originalScale, hoverDuration, Ease.OutQuad);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (enablePress)
            Tween.Scale(rectTransform, originalScale * pressScale, pressDuration, Ease.OutQuad);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (enablePress)
            Tween.Scale(rectTransform, originalScale, pressDuration, Ease.OutQuad);
    }
}