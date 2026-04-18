using System;
using System.Collections.Generic;
using System.Linq;
using Data;
using UI;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class TrafficManager : MonoBehaviour
{
    [SerializeField] private PacketCard _cardPrefab;
    [SerializeField] private TrafficEntry[] _traffic;

    [SerializeField] private CardContainer _cardContainer;
    
    private readonly List<TrafficEntry.PacketPoolEntry> _cards = new();

    private int _currentTraffic = -1;
    private float _timestamp = 0;
    private float _interval = 0;

    private void Start()
    {
        NextTraffic();
    }

    private void NextTraffic()
    {
        _currentTraffic = Mathf.Clamp(_currentTraffic + 1, 0, _traffic.Length-1);
        _cards.Clear();
        FillCards();
    }

    private void Update()
    {
        
        if (Time.time > _timestamp + _interval)
        {
            _timestamp = Time.time;
            NextCard();
        }
    }

    private void FillCards()
    {
        _cards.AddRange(_traffic[_currentTraffic].PacketPool.Select(packet => packet));
    }

    private void NextCard()
    {
        if (_cardContainer.FreeSlotCount - 1 <= 0)
        {
            return;
        }
        if (_cards.Count == 0)
        {
            FillCards();
        }
        var data = PopRandomPacket();
        var current = _traffic[_currentTraffic];
        var card = Instantiate(_cardPrefab, transform);
        card.Init(data);
        _cardContainer.AddCard(card);
        _interval = Random.Range(current.IntervalMin, current.IntervalMax);
    }

    public IPacketData PopRandomPacket()
    {
        var weightSum = _cards.Select(card => card.Weight).Sum();
        var rand = Random.Range(0, weightSum);
        float counter = 0;
        for (int i = 0; i < _cards.Count; i++)
        {
            counter += _cards[i].Weight;
            if (counter >= rand)
            {
                var packet = _cards[i];
                _cards.RemoveAt(i);
                return packet.PacketData;
            }
        }

        {
            var packet = _cards[^1];
            _cards.RemoveAt(_cards.Count - 1);
            return packet.PacketData;
        }
    }

    [Serializable]
    public class TrafficEntry
    {
        [SerializeField] private int _pointsToComplete;
        [SerializeField] private PacketPoolEntry[] _packetPool;
        [SerializeField] private float _intervalMin = 10f;
        [SerializeField] private float _intervalMax = 20f;
        
        public int PointsToComplete => _pointsToComplete;
        public PacketPoolEntry[] PacketPool => _packetPool;
        public float IntervalMin => _intervalMin;
        public float IntervalMax => _intervalMax;
        
        [Serializable]
        public class PacketPoolEntry
        {
            [SerializeField] private float _weight;
            [SerializeField] private PacketData _packetData;
            
            public PacketData PacketData => _packetData;
            public float Weight => _weight;
        }
    }
}
