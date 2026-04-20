using System;
using System.Collections.Generic;
using System.Linq;
using Data;
using UnityEngine;

namespace UI
{
    public class CardContainer : MonoBehaviour
    {
        [SerializeField] private Transform[] _slots;
        [SerializeField] private bool _useMaxUpgrade;
        [SerializeField] private UpgradeType _upgradeType;
        [SerializeField] protected Game Game;
        
        public int MaxSlots => _slots.Length;
        public int ContainerSize => _slots.Length - (_useMaxUpgrade && !Game.Upgrades.Contains(_upgradeType) ? 1 : 0);
        public int CardCount => _cards.Count(c => c);
        public int FreeSlotCount => ContainerSize - CardCount;
        private PacketCard[] _cards;
        
        protected PacketCard[] Cards => _cards;

        protected virtual void Awake()
        {
            _cards = new PacketCard[MaxSlots];
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

        protected virtual void Update()
        {
            _slots[^1].gameObject.SetActive(!(_useMaxUpgrade && !Game.Upgrades.Contains(_upgradeType)));
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
            for (int i = 0; i < ContainerSize; i++)
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