using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UpgradesListEntry : MonoBehaviour
    {
        [SerializeField] private Image _image;
        [SerializeField] private TextMeshProUGUI _text;

        public Image Image => _image;
        public TextMeshProUGUI Text => _text;
    }
}