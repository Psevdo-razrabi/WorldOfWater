using Game.Services;
using ObservableCollections;
using R3;
using VContainer.Unity;
using UnityEngine;
using VContainer;

public class ChatService : ITickable
{
    private ViewsService _viewsService;
    public ReactiveProperty<bool> Opened = new();
    
    public readonly ObservableQueue<string> Messages = new();

    [Inject]
    private void Construct(ViewsService viewsService)
    {
        _viewsService = viewsService;
    }
    
    public void Tick()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            Opened.Value = !Opened.Value;
            if (Opened.Value)
            {
                //SendMessage();
            }
            else
            {
                _viewsService.Open<ChatView>();
            }
        }
    }

    public void SendMessage(string message)
    {
        if (string.IsNullOrEmpty(message))
        {
            if (Messages.Count >= 5)
            {
                Messages.Dequeue();
            }
            Messages.Enqueue(message);
        }

        _viewsService.Close();
    }
}
