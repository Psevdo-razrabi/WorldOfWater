using System;
using UniRx;
using UnityEngine;

namespace Helpers.Timer
{
    public class Timer
    {
        private readonly float _time;
        private readonly Action _delegateWithTimerStop;
        private readonly CompositeDisposable _compositeDisposable = new();

        public Timer(float time, Action delegateWithTimerStop)
        {
            _time = time;
            _delegateWithTimerStop = delegateWithTimerStop;
        }

        public void Start()
        {
            Observable
                .Timer(TimeSpan.FromSeconds(_time))
                .Subscribe(_ =>
                {
                    _delegateWithTimerStop();
                    Debug.Log($"конец бафа");
                    Dispose();
                    
                }).AddTo(_compositeDisposable);
        }

        public void Dispose()
        {
            _compositeDisposable?.Clear();
            _compositeDisposable?.Dispose();
        }
    }
}