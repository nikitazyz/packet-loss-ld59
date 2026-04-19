using UnityEngine;

namespace UI
{
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