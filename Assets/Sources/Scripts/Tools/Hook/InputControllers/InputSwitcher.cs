namespace Sources.Scripts.Hook.InputControllers
{
    public class InputSwitcher
    {
        private IInputController _currentController;

        public void SwitchController(IInputController inputController)
        {
            if (_currentController != null)
                _currentController.Disable();
            
            _currentController = inputController;
            _currentController.Enable();
        }
    }
}