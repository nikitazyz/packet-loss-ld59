using System;
using Data;
using PrimeTween;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace UI
{
    public class PacketCard : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        public static event Action<PacketCard, PointerEventData> OnDrop;
        [SerializeField] private GameObject _mainPanel;
        [SerializeField] private Image _background;
        [SerializeField] private ProgressBar _bar;
        [SerializeField] private TextMeshProUGUI _label;
        [SerializeField] private TextMeshProUGUI _weightText;

        [SerializeField] private PacketResourcePack _resourcePack;
        
        public CardContainer CurrentContainer { get; set; }

        private bool _isVirus;
        private bool _isSuspicious;
        private float _time;
        private int _weight;
        private IPacketData _data;

        public float RemainingTime => _time;
        public bool IsVirus => _isVirus;
        public bool IsSuspicious => _isSuspicious;
        public int Weight => _weight;
        public bool CanDrag { get; set; } = true;

        private Vector2 _mouseOffset;
        private bool _isDragging;
        
        public void Init(IPacketData packetData)
        {
            _data = packetData;
            _isVirus = packetData.IsVirus;
            _isSuspicious = packetData.IsSuspicious;
            _weight = packetData.Weight;
            _time = packetData.Time;

            _label.text = CreateLabel();
            _weightText.text = _weight.ToString() + "b";
            UpdateUI();
            AnimatePopup();
        }

        private void AnimatePopup()
        {
            _mainPanel.transform.localScale = Vector3.zero;
            Tween.Scale(_mainPanel.transform, new TweenSettings<float>(1f, 0.2f, Ease.OutQuad));
        }

        private string CreateLabel()
        {
            var textSet = _resourcePack.PacketNames;
            string label = textSet[Random.Range(0, textSet.Length)];

            label += $"_x{Random.Range(0, 0xffff):x4}";
            return label;
        }

        private void UpdateUI()
        {
            Sprite bg = _resourcePack.Normal;
            if (_isVirus)
                bg = _resourcePack.Virus;
            
            if (_isSuspicious)
                bg = _resourcePack.Suspicious;

            _background.sprite = bg;
            _bar.Value = _time / _data.Time;
        }

        public void CheckPacket()
        {
            _isSuspicious = false;
            UpdateUI();
        }

        public void UpdateCard(float deltaTime)
        {
            _time -= deltaTime;
            _bar.Value = _time / _data.Time;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!_isDragging)
            {
                return;
            }
            _mainPanel.transform.position = eventData.position + _mouseOffset;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (!CanDrag)
            {
                return;
            }
            _isDragging = true;
            _mouseOffset = (Vector2)_mainPanel.transform.position - eventData.position;
            transform.SetAsLastSibling();
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (!_isDragging)
            {
                return;
            }
            _isDragging = false;
            _mainPanel.transform.localPosition = Vector3.zero;
            OnDrop?.Invoke(this, eventData);
        }
    }
}
