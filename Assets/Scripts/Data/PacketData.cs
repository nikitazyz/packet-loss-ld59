using UnityEngine;

namespace Data
{
    
    [CreateAssetMenu(menuName = "Packet Data", fileName = "PacketData", order = 0)]
    public class PacketData : ScriptableObject, IPacketData
    {
        [field: SerializeField]
        public int Weight { get; private set; }
        [field: SerializeField]
        public float Time { get; private set; }
        [field: SerializeField]
        public bool IsVirus { get; private set; }
        [field: SerializeField]
        public bool IsSuspicious { get; private set; }
    }
}