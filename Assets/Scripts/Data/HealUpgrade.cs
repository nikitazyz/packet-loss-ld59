using UnityEngine;

namespace Data
{
    [CreateAssetMenu(menuName = "HealUpgrade", fileName = "HealUpgrade", order = 0)]
    public class HealUpgrade : FlagUpgrade
    {
        [SerializeField] private int maxHealth;
        public override void Execute(Game game)
        {
            base.Execute(game);
            game.SetMaxHealth(maxHealth);
        }
    }
}