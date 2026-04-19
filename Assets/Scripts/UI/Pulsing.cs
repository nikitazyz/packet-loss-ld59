using PrimeTween;
using UnityEngine;

namespace UI
{
    public class Pulsing : MonoBehaviour
    {
        [SerializeField] private float _scaleFactor;
        [SerializeField] private float _pulsingTime;
    
        void Start()
        {
            Tween.Scale(transform, new TweenSettings<float>(_scaleFactor, _pulsingTime, Ease.InOutSine, -1, CycleMode.Yoyo));
        }
    }
}
