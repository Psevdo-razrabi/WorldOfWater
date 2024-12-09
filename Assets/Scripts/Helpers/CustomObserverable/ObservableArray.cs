using System;
using System.Collections.Generic;
using System.Linq;
using Inventory;
using R3;

namespace Helpers
{
    public class ObservableArray<T> : IObservableArray<T>, IDisposable
    {
        private T[] _elements;
        private Subject<T[]> _valueChangeInArray { get; } = new();
        public Observable<T[]> ValueChangeInArray => _valueChangeInArray;
        public int Count => _elements.Count(element => element != null);
        public T this[int index] => _elements[index];

        public ObservableArray(int size, IList<T> list = null)
        {
            _elements = new T[size];
            ConvertInitListToArray(list, size);
        }

        public void Swap(int indexOne, int indexTwo)
        {
            (_elements[indexOne], _elements[indexTwo]) = (_elements[indexTwo], _elements[indexOne]);
            OnNext();
        }

        public void Clear()
        {
            _elements = new T[_elements.Length];
            OnNext();
        }

        public T[] GetArray()
        {
            return _elements;
        }

        public void Update()
        {
            OnNext();
        }

        public bool TryAdd(T element)
        {
            if (HasFreeSpase() == false)
            {
                EnlargeArray();
            }
            
            for (int i = 0; i < _elements.Length; i++)
            {
                if (_elements[i] != null) continue;
                _elements[i] = element;
                OnNext();
                return true;
            }

            return false;
        }

        private void EnlargeArray()
        {
            var array = new T[_elements.Length * 2];
            Array.Copy(_elements, array, _elements.Length);
            _elements = array;
        }

        public bool TryRemove(T element)
        {
            for (int i = 0; i < _elements.Length; i++)
            {
                if(EqualityComparer<T>.Default.Equals(_elements[i], element) == false) continue;
                _elements[i] = default;
                OnNext();
                return true;
            }

            return false;
        }

        private void ConvertInitListToArray(IList<T> list, int size)
        {
            if (list == null) return;
            
            list.Take(size).ToArray().CopyTo(_elements, 0);
            OnNext();
        }

        private void OnNext() => _valueChangeInArray.OnNext(_elements);

        private bool HasFreeSpase() => _elements.Any(element => element == null);

        public void Dispose()
        {
            _valueChangeInArray?.Dispose();
        }
    }
}