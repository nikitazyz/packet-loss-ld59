using System;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class CardContainer : MonoBehaviour
    {
        [SerializeField] private int _containerSize = 1;
        
        public int ContainerSize => _containerSize;
        public int CardCount => _cards.Count;
        public int FreeSlotCount => ContainerSize - CardCount;
        private readonly HashSet<PacketCard> _cards = new();


        public void AddCard(PacketCard card)
        {
            if (_cards.Add(card))
            {
                card.transform.SetParent(transform);
            }
        }

        public void RemoveCard(PacketCard card)
        {
            _cards.Remove(card);
            card.transform.SetParent(null);
        }

        private void Update()
        {
            foreach (var card in _cards)
            {
                card.UpdateCard(Time.deltaTime);
            }
        }
    }
}