using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Domain
{
    public class WindowEnumerator<T> : IEnumerator
    {
        private readonly IEnumerable<T> _data;
        private readonly int _windowSize;
        private int _iteration = 0;

        public IEnumerable<T> Current { get; private set; } = null;
        object IEnumerator.Current => Current;

        public WindowEnumerator(IEnumerable<T> data, int windowSize)
        {
            _data = data ?? throw new Exceptions.DataIsNotDeterminedException();
            _windowSize = windowSize > 0 ? windowSize : throw new Exceptions.WindowSizeMustBeGreaterThanZeroException();
        }

        public bool MoveNext()
        {
            Current = _data.Skip(_iteration++).Take(_windowSize);
            return Current.Count() == _windowSize;
        }

        public void Reset()
        {
            _iteration = 0;
        }

        public static class Exceptions
        {
            public class DataIsNotDeterminedException : ArgumentNullException { }
            public class WindowSizeMustBeGreaterThanZeroException : ArgumentException { }
        }
    }
}
