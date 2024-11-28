using Sources.Scripts.UI.Interfaces;
using Sources.Scripts.UI.ThrowToolsUI.Hook;
using UnityEngine;

namespace Sources.Scripts.UI.ThrowToolsUI
{
    public class ThrowToolsPresenter : IThrowable
    {
        private ThrowToolsModel _throwToolsModel;
        private CircleLoadbarView _circleLoadbarView;
        
        public ThrowToolsPresenter(CircleLoadbarView circleLoadbarView)
        {
            _circleLoadbarView = circleLoadbarView;
        }

        public void GetModel(ThrowToolsModel throwToolsModel)
        {
            Debug.Log("ааа");
            _throwToolsModel = throwToolsModel;
        }
        
        public void ThrowPerfermed()
        {
            if(_throwToolsModel.IsThrowed == false)
                _circleLoadbarView.StartLoading(_throwToolsModel.ToolConfig.SpeedFillCircle);
        }

        public void ThrowCanceled()
        {
            _throwToolsModel.ToolThrowed(_circleLoadbarView.FillAmount);
            _circleLoadbarView.CancelLoading();
        }
    }
}