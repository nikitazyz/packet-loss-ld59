using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UI
{
    public class CardContainer : MonoBehaviour
    {
        [SerializeField] private Transform[] _slots;
        
        public int ContainerSize => _slots.Length;
        public int CardCount => _cards.Count(c => c);
        public int FreeSlotCount => ContainerSize - CardCount;
        private PacketCard[] _cards;
        
        protected PacketCard[] Cards => _cards;

        protected virtual void Awake()
        {
            _cards = new PacketCard[ContainerSize];
        }

        public void AddCard(PacketCard card)
        {
            var slot = GetEmptySlot();
            if (slot >= 0 && !HasCard(card))
            {
                _cards[slot] = card;
                card.transform.position = _slots[slot].position;
                card.CurrentContainer = this;
                OnCardAdded(card, slot);
            }
        }

        protected virtual void OnCardAdded(PacketCard card, int slot){}

        public void RemoveCard(PacketCard card)
        {
            var index = Array.IndexOf(_cards, card);
            if (index >= 0)
            {
                _cards[index] = null;
                card.CurrentContainer = null;
            }
        }

        public bool HasCard(PacketCard card)
        {
            return _cards.Contains(card);
        }

        private int GetEmptySlot()
        {
            for (int i = 0; i < _cards.Length; i++)
            {
                if (!_cards[i])
                {
                    return i;
                }
            }

            return -1;
        }
        
        public void Clear()
        {
            foreach (var card in _cards)
            {
                if (card)
                {
                    RemoveCard(card);
                    Destroy(card.gameObject);
                }
            }
        }
    }
}