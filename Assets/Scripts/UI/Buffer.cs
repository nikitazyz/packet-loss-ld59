using System;
using Data;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    public class Buffer : CardContainer
    {
        public event Action<PacketCard> SendRequest;
        public event Action<PacketCard> OnCardExpired;
        
        protected override void Awake()
        {
            base.Awake();
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
        
        protected override void Update()
        {
            foreach (var card in Cards)
            {
                if (!card) continue;
                
                card.UpdateCard(Time.deltaTime * (Game.Upgrades.Contains(UpgradeType.BufferTime) ? 0.5f : 1f));
                if (card.RemainingTime <= 0)
                {
                    OnCardExpired?.Invoke(card);
                }
            }
        }
    }
}