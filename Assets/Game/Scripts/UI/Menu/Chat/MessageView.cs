using Game.MVVM;
using TMPro;
using UnityEngine;

public class MessageView : View
{
    [SerializeField] private TMP_Text _text;
    
    public void Init(string text)
    {
        _text.text = text;
    }
}