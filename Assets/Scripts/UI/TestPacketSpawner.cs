using System;
using Data;
using UnityEngine;

namespace UI
{
    public class TestPacketSpawner : MonoBehaviour
    {
        [SerializeField] private PacketCard _prefab;
        [SerializeField] private PacketData _data;

        private void Start()
        {
            var card = Instantiate(_prefab, transform);
            card.Init(_data);
        }
    }
}