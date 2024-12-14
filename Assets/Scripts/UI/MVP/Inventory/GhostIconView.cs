using Helpers;
using UnityEngine;
using UnityEngine.UI;

namespace Inventory
{
    public class GhostIconView : MonoBehaviour
    {
        [field: SerializeField] public Image Image { get; private set; }
        public Sprite Sprite { get; private set; }

        public void SetImage(Sprite image)
        {
            Preconditions.CheckNotNull(image);
            Sprite = image;
            Image.sprite = image;
            Image.color = Color.white;
        }

        public void Clear()
        {
            Sprite = null;
            Image.sprite = default;
            Image.color = new Color(1, 1, 1, 0f);
        }
    }
}