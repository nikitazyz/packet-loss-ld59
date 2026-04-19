using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace UI
{
    public class Parallax : MonoBehaviour
    {
        [SerializeField] private float _parallaxAmplitude;

        private void Update()
        {
            var mousePos = Mouse.current.position.ReadValue();
            var centerPos = new Vector2(Screen.width / 2f, Screen.height / 2f);
            var offset = mousePos - centerPos;
            offset.x /= Screen.width;
            offset.y /= Screen.height;
            
            transform.localPosition = offset * _parallaxAmplitude;
        }
    }
}
