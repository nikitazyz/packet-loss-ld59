using UnityEngine;

namespace UI
{
    
    [CreateAssetMenu(menuName = "Game Resource Pack", fileName = "GameResourcePack", order = 0)]
    public class GameResourcePack : ScriptableObject
    {
        [SerializeField] private Sprite _heart;
        [SerializeField] private Sprite _emptyHeart;

        public Sprite Heart => _heart;
        public Sprite EmptyHeart => _emptyHeart;
    }
}