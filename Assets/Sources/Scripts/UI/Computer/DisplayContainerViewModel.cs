using Game.Services;
using R3;
using Zenject;

namespace Game.MVVM
{
    public class DisplayContainerViewModel : ViewModel
    {
        [Inject]
        private void Construct(ContractsService contractsService)
        {
        }

        public void Init(IBindable button)
        {
            Binder.CreateButtonEvent<ClickBinderEvent>(button, OnClicked);
        }

        private void OnClicked()
        {
            //Binder.ViewTriggered.Execute();
            Binder.TriggerView();
        }
    }
}