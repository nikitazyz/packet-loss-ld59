using System;
using Data;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace UI
{
    public class SendingContainer : CardContainer
    {
        public event Action<PacketCard> OnCardSent;
        public event Action<PacketCard> SendRequest;
        
        [SerializeField] private ProgressBar[] _progressBars;
        [SerializeField] private float _time;
        [SerializeField] private UpgradeType _speed;
        [SerializeField] private UpgradeType _instant;

        [SerializeField] private GameEventType _eventType;

        private float[] _sendingProgress;
        protected override void Awake()
        {
            base.Awake();
            _sendingProgress = new float[MaxSlots];
            PacketCard.OnDrop += OnPacketDrop;
        }

        private void OnDestroy()
        {
            PacketCard.OnDrop -= OnPacketDrop;
        }

        private void OnPacketDrop(PacketCard packet, PointerEventData eventData)
        {
            if (FreeSlotCount == 0)
            {
                return;
            }
            var rectTransform = GetComponent<RectTransform>();
            
            Vector3[] corners = new Vector3[4];
            rectTransform.GetWorldCorners(corners);

            Vector2 min = new Vector2(float.MaxValue, float.MaxValue);
            Vector2 max = new Vector2(float.MinValue, float.MinValue);

            for (int i = 0; i < 4; i++)
            {
                Vector3 screenPoint = RectTransformUtility.WorldToScreenPoint(null, corners[i]);

                min = Vector2.Min(min, screenPoint);
                max = Vector2.Max(max, screenPoint);
            }

            var mpos = eventData.position;
            
            if (mpos.x < min.x ||  mpos.x > max.x)
                return;
            if (mpos.y < min.y ||  mpos.y > max.y)
                return;
            
            SendRequest?.Invoke(packet);
        }

        protected override void OnCardAdded(PacketCard card, int slot)
        {
            _sendingProgress[slot] = 0;
            _progressBars[slot].Value = _sendingProgress[slot];
        }

        protected override void Update()
        {
            base.Update();
            for (int i = 0; i < ContainerSize; i++)
            {
                var card = Cards[i];
                if (!card)
                {
                    _progressBars[i].gameObject.SetActive(false);
                    continue;
                }
                _sendingProgress[i] += Time.deltaTime * (Game.Upgrades.Contains(_speed) ? 3 : 1) * (Game.EventType == _eventType ? 0.75f : 1);
                _progressBars[i].gameObject.SetActive(true);
                _progressBars[i].Value = _sendingProgress[i] / (_time * card.Weight);
                if (_sendingProgress[i] >= _time * card.Weight || Game.Upgrades.Contains(_instant))
                {
                    OnCardSent?.Invoke(card);
                }
            }
        }
    }
}