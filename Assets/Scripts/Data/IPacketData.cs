using System;
using UnityEngine;

namespace Data
{
    public interface IPacketData
    {
        public int Weight { get; }
        public float Time { get; }
        public bool IsVirus { get; }
        public bool IsSuspicious { get; }
    }
}