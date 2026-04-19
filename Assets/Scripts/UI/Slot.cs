using System;
using UnityEngine;

namespace UI
{
    public class Slot : MonoBehaviour
    {
        private PacketCard _card;
        
        public void AddCard(PacketCard card)
        {
            if (_card)
            {
                return;
            }
            _card = card;
        }

        public PacketCard GetCard()
        {
            return _card;
        }

        public void RemoveCard()
        {
            _card = null;
        }

        private void Update()
        {
            _card.transform.position = transform.position;
        }
    }
}