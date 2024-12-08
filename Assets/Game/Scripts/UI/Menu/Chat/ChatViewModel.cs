using System.Collections.Generic;
using System.Linq;
using Game.MVVM;
using ObservableCollections;
using R3;
using VContainer;

public class ChatViewModel : ViewModel
{
    private ChatService _chatService;
    public List<string> Messages => _chatService.Messages.ToList();

    [Inject]
    public void Construct(ChatService chatService)
    {
        _chatService = chatService;
    }

    public void Init()
    {
        var test = _chatService.Messages.ObserveChanged();
        test.Subscribe(OnUpdated);
    }

    public void SendMessage(string message)
    {
        _chatService.SendMessage(message);
    }

    private void OnUpdated(CollectionChangedEvent<string> value)
    {
        Messages.Clear();
        foreach (var message in _chatService.Messages)
        {
            Messages.Add(message);
            //(View as ChatView)?.DisplayMessage(message);
            Binder.TriggerView();
        }
    }
}