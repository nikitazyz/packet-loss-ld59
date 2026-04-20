using UnityEngine;

namespace Data
{
    [CreateAssetMenu(menuName = "Flag Upgrade", fileName = "FlagUpgrade", order = 0)]
    public class FlagUpgrade : ScriptableObject, IUpgrade
    {
        [field: SerializeField]
        public string Name { get; private set; }

        [field: SerializeField]
        public UpgradeType UpgradeType { get; private set; }
        
        
        public virtual void Execute(Game game)
        {
            game.Upgrades.Add(UpgradeType);
        }
    }
}