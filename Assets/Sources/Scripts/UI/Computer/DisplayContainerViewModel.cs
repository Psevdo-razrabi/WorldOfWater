using Game.Services;
using R3;
using VContainer;

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
            Binder.CreateButtonTrigger<Click>(button, OnClicked);
        }

        private void OnClicked()
        {
            //Binder.ViewTriggered.Execute();
            Binder.TriggerView();
        }
    }
}