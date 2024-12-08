using System.Collections.Generic;
using Game.MVVM;
using TMPro;
using UnityEngine;
using Button = Game.MVVM.Button;

public class ChatView : View<ChatViewModel>
{
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private Button sendButton;
    [SerializeField] private Transform messagesContainer;
    [SerializeField] private MessageView messagePrefab;

    private List<MessageView> Messages = new();

    public override void Open()
    {
        ViewModel.Init();
        Binder.CreateButtonTrigger<Click>(sendButton, OnMessageSent);
        SubscribeUpdateView(UpdateView);
    }

    public void OnMessageSent()
    {
        ViewModel.SendMessage(inputField.text);
        inputField.text = string.Empty;
    }
    
    public void UpdateView()
    {
        Messages.Clear();
        foreach (var message in ViewModel.Messages)
        {
            var messageObject = Instantiate(messagePrefab, messagesContainer);
            messageObject.Init(message);
            Messages.Add(messageObject);
        }
    }
}