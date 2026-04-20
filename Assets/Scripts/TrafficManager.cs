using System;
using System.Collections.Generic;
using System.Linq;
using Data;
using UI;
using UnityEngine;
using Buffer = UI.Buffer;
using Random = UnityEngine.Random;

public class TrafficManager : MonoBehaviour
{
    [SerializeField] private Game _game;
    
    [SerializeField] private PacketCard _cardPrefab;
    [SerializeField] private TrafficEntry[] _traffic;

    [SerializeField] private Buffer _buffer;
    [SerializeField] private SendingContainer _sendingContainer;
    [SerializeField] private SendingContainer _deletingContainer;
    [SerializeField] private SendingContainer _verifyingContainer;

    [SerializeField] private UpgradePanel _upgrades;
    [SerializeField] private NoUpgradesPanel _noUpgradesPanel;
    
    public TrafficEntry CurrentTraffic => _currentTraffic >= 0 ? _traffic[_currentTraffic] : null;
    
    private readonly List<TrafficEntry.PacketPoolEntry> _cards = new();

    private int _currentTraffic = -1;
    private float _timestamp;
    private float _interval;

    private bool _isUpgradeSelecting;
    private bool _stopUpgradeSelection;
    private bool _noUpgradesShown;

    private void Awake()
    {
        _buffer.SendRequest += card => OnSendRequest(_buffer, card);
        _sendingContainer.SendRequest += card => OnSendRequest(_sendingContainer, card);
        _deletingContainer.SendRequest += card => OnSendRequest(_deletingContainer, card);
        _verifyingContainer.SendRequest += card => OnSendRequest(_verifyingContainer, card);

        _buffer.OnCardExpired += CardExpired;
        _sendingContainer.OnCardSent += CardSent;
        _deletingContainer.OnCardSent += CardDeleted;
        _verifyingContainer.OnCardSent += CardVerified;

        _game.OnTakePoint += OnTakePoint;
    }

    private void OnTakePoint()
    {
        if (_game.Points >= _traffic[_currentTraffic].PointsToComplete)
        {
            ClearCards();
            if (!_stopUpgradeSelection)
            {
                SelectUpgrade();
                if (_currentTraffic+1 >= _traffic.Length)
                {
                    _stopUpgradeSelection = true;
                }
            }
            else
            {
                if (!_noUpgradesShown)
                {
                    _isUpgradeSelecting = true;
                    _noUpgradesPanel.ShowPanel(() =>
                    {
                        _noUpgradesShown = true;
                        _isUpgradeSelecting = false;
                    });
                }
            }
            NextTraffic();
        }
    }

    private void SelectUpgrade()
    {
        _isUpgradeSelecting = true;
        var current = _traffic[_currentTraffic];
        _upgrades.ShowUpgradePanel(current.FlagUpgrade1, current.FlagUpgrade2, () => _isUpgradeSelecting = false);
    }

    private void CardVerified(PacketCard card)
    {
        card.CheckPacket();
        if (_buffer.FreeSlotCount <= 0)
        {
            return;
        }
        card.CurrentContainer?.RemoveCard(card);
        _buffer.AddCard(card);
    }

    private void CardDeleted(PacketCard card)
    {
        if (!card.IsVirus)
        {
            _game.TakeDamage();
        }
        else
        {
            _game.AddPoint();
        }
        card.CurrentContainer?.RemoveCard(card);
        Destroy(card.gameObject);
    }

    private void CardSent(PacketCard card)
    {
        if (card.IsVirus)
        {
            _game.TakeDamage();
        }
        else
        {
            _game.AddPoint();
        }
        
        card.CurrentContainer?.RemoveCard(card);
        Destroy(card.gameObject);
    }

    private void CardExpired(PacketCard card)
    {
        card.CurrentContainer?.RemoveCard(card);
        _game.TakeDamage();
        Destroy(card.gameObject);
    }

    private void OnSendRequest(CardContainer container, PacketCard card)
    {
        if (!card || card.CurrentContainer == container) return;
        card.CurrentContainer?.RemoveCard(card);
        container.AddCard(card);
    }

    private void Start()
    {
        NextTraffic();
    }

    private void NextTraffic()
    {
        Debug.Log("NextTraffic");
        _currentTraffic = Mathf.Clamp(_currentTraffic + 1, 0, _traffic.Length-1);
        _game.ClearPoints();
        _game.CountStage();
        FillCards();
    }

    private void ClearCards()
    {
        _cards.Clear();
        _buffer.Clear();
        _sendingContainer.Clear();
        _deletingContainer.Clear();
        _verifyingContainer.Clear();
    }

    private void Update()
    {
        if (Time.time > _timestamp + _interval && !_isUpgradeSelecting)
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
        if (_buffer.FreeSlotCount - 1 <= 0)
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
        _buffer.AddCard(card);
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
        [SerializeField] private FlagUpgrade _flagUpgrade1;
        [SerializeField] private FlagUpgrade _flagUpgrade2;
        
        public int PointsToComplete => _pointsToComplete;
        public PacketPoolEntry[] PacketPool => _packetPool;
        public float IntervalMin => _intervalMin;
        public float IntervalMax => _intervalMax;
        
        public FlagUpgrade FlagUpgrade1 => _flagUpgrade1;
        public FlagUpgrade FlagUpgrade2 => _flagUpgrade2;
        
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
