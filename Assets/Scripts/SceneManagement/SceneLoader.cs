using Cysharp.Threading.Tasks;
using DG.Tweening;
using Loader;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace SceneManagment
{
    public class SceneLoader : MonoBehaviour
    {
        [SerializeField] private SceneGroup _sceneGroup;
        [SerializeField] private Slider _slider;
        [SerializeField] private float _fadeDuration = 0.4f;
        [SerializeField] private float _minSlider = 0.05f;
        [SerializeField] private CanvasGroup _group;
        [SerializeField] float fillSpeed = 0.5f;

        private SceneManager _sceneManager;
        private float _targetProgress;
        private bool _isLoading;

        [Inject]
        private void Construct(SceneResources sceneResources, SceneManager sceneManager)
        {
            _sceneManager = sceneManager;
            _sceneManager.Construct(sceneResources);
        }

        private void Update()
        {
            if(_isLoading == false) return;

            var currentFillAmount = _slider.value;
            var progressDifference = Mathf.Abs(currentFillAmount - _targetProgress);
            var dynamicFillSpeed = progressDifference * fillSpeed;

            _slider.value = Mathf.Lerp(currentFillAmount, _targetProgress, Time.deltaTime * dynamicFillSpeed);
        }

        private void ChangeParameters()
        {
            _group.alpha = 1f;
            _slider.value = _minSlider;
            _targetProgress = 1f;
            _group.interactable = true;
            _group.blocksRaycasts = true;
        }

        public async UniTask LoadScene(TypeScene typeScene)
        {
            ChangeParameters();
            LoadingProgress loadingProgress = new LoadingProgress();
            loadingProgress.Progressed += value => _targetProgress = Mathf.Max(value, _targetProgress);
            _isLoading = true;
            await _sceneManager.LoadScene(_sceneGroup, loadingProgress, typeScene);
            _isLoading = false;
        }

        public async void Animation(SceneManager.LoadSceneDelegate loadSceneDelegate, string type)
        {
            await DOTween.To(() => _group.alpha, x => _group.alpha = x, 1, _fadeDuration)
                .SetUpdate(UpdateType.Late, true)
                .OnComplete(() => loadSceneDelegate.Invoke(type));
        }

        public void FadeOut()
        {
            _group.interactable = false;
            _group.blocksRaycasts = false;
            DOTween.To(() => _group.alpha, x => _group.alpha = x, 0, _fadeDuration).SetUpdate(UpdateType.Late, true);
        }
    }
}