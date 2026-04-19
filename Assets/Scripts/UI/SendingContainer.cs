using System;
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

        private float[] _sendingProgress;
        protected override void Awake()
        {
            base.Awake();
            _sendingProgress = new float[ContainerSize];
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
            Debug.Log("Packet drop");
            var rectTransform = GetComponent<RectTransform>();

            var min = rectTransform.rect.min;
            var max = rectTransform.rect.max;
            var pos = (Vector2)transform.position;

            var minBound = pos + min;
            var maxBound = pos + max;

            var mpos = eventData.position;

            Debug.Log($"min: {min},  max: {max}, pos: {pos}");
            Debug.Log($"Mouse: {mpos}, min: {minBound}, max: {maxBound}");
            
            if (mpos.x < minBound.x ||  mpos.x > maxBound.x)
                return;
            if (mpos.y < minBound.y ||  mpos.y > maxBound.y)
                return;
            
            SendRequest?.Invoke(packet);
        }

        protected override void OnCardAdded(PacketCard card, int slot)
        {
            _sendingProgress[slot] = 0;
            _progressBars[slot].Value = _sendingProgress[slot];
        }

        private void Update()
        {
            for (int i = 0; i < ContainerSize; i++)
            {
                var card = Cards[i];
                if (!card)
                {
                    _progressBars[i].gameObject.SetActive(false);
                    continue;
                }
                _sendingProgress[i] += Time.deltaTime;
                _progressBars[i].gameObject.SetActive(true);
                _progressBars[i].Value = _sendingProgress[i] / (_time * card.Weight);
                if (_sendingProgress[i] >= _time * card.Weight)
                {
                    OnCardSent?.Invoke(card);
                }
            }
        }
    }
}