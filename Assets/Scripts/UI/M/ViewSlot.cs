using Helpers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ViewSlot : MonoBehaviour
{
    [field: SerializeField] public Image Image { get; private set; }
    [field: SerializeField] public TextMeshProUGUI _stackLabel { get; private set; }
    public int Index { get; private set; }
    public Sprite Sprite { get; private set; }
    public GuidItem GuidItem { get; private set; } = GuidItem.IsEmpty();

    public void SetIndex(int index)
    {
        Preconditions.CheckValidateData(index);
        Index = index;
    }

    public void SetImage(Sprite image)
    {
        Preconditions.CheckNotNull(image);
        Sprite = image;
        Image.sprite = image;
        Image.color = Color.white;
    }

    public void SetStackLabel(string text)
    {
        Preconditions.CheckNotNull(text);
        _stackLabel.text = text;
    }

    public void SetGuid(GuidItem guidItem)
    {
        GuidItem = guidItem;
    }

    public void Clear()
    {
        Sprite = null;
        Image.sprite = default;
        Image.color = new Color(1, 1, 1, 0f);
        _stackLabel.text = default;
        GuidItem = GuidItem.IsEmpty();
    }
}
