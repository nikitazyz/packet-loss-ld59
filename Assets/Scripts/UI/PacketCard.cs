using Data;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
    public class PacketCard : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        [SerializeField] private GameObject _mainPanel;
        [SerializeField] private Image _background;
        [SerializeField] private ProgressBar _bar;
        [SerializeField] private TextMeshProUGUI _label;
        [SerializeField] private TextMeshProUGUI _weightText;

        [SerializeField] private PacketResourcePack _resourcePack;

        private bool _isVirus;
        private bool _isSuspicious;
        private float _time;
        private int _weight;
        private IPacketData _data;

        public float RemainingTime => _time;
        public bool IsVirus => _isVirus;
        public bool IsSuspicious => _isSuspicious;
        public int Weight => _weight;

        private Vector2 _mouseOffset;
        private bool _isDragging;
        
        public void Init(IPacketData packetData)
        {
            _data = packetData;
            _isVirus = packetData.IsVirus;
            _isSuspicious = packetData.IsSuspicious;
            _weight = packetData.Weight;
            _time = packetData.Time;

            _label.text = CreateLabel();
            _weightText.text = _weight.ToString() + "b";
            UpdateUI();
        }

        private string CreateLabel()
        {
            var textSet = _resourcePack.PacketNames;
            string label = textSet[Random.Range(0, textSet.Length)];

            label += $"_x{Random.Range(0, 0xffff):x4}";
            return label;
        }

        private void UpdateUI()
        {
            Sprite bg = _resourcePack.Normal;
            if (_isVirus)
                bg = _resourcePack.Virus;
            
            if (_isSuspicious)
                bg = _resourcePack.Suspicious;

            _background.sprite = bg;
            _bar.Value = _time / _data.Time;
        }

        public void CheckPacket()
        {
            _isSuspicious = false;
            UpdateUI();
        }

        public void UpdateCard(float deltaTime)
        {
            _time -= deltaTime;
            _bar.Value = _time / _data.Time;
        }

        public void OnDrag(PointerEventData eventData)
        {
            _mainPanel.transform.position = eventData.position + _mouseOffset;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            _isDragging = true;
            _mouseOffset = (Vector2)_mainPanel.transform.position - eventData.position;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            _isDragging = false;
            _mainPanel.transform.localPosition = Vector3.zero;
        }
    }

    [CreateAssetMenu(menuName = "Packet Resource Pack", fileName = "PacketResourcePack", order = 0)]
    public class PacketResourcePack : ScriptableObject
    {
        [SerializeField] private Sprite _normal;
        [SerializeField] private Sprite _suspicious;
        [SerializeField] private Sprite _virus;

        [SerializeField] private string[] _packetNames;
        
        public Sprite Normal => _normal;
        public Sprite Suspicious => _suspicious;
        public Sprite Virus => _virus;
        
        public string[] PacketNames => _packetNames;
    }
}
