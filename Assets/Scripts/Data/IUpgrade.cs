using UnityEngine;

namespace Data
{
    public interface IUpgrade
    {
        public string Name { get; }
        public UpgradeType UpgradeType { get; }
        public void Execute(Game game);
    }
}