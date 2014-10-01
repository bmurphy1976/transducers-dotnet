using System;

namespace TransducersNet
{
    public class FuncFilter<T> : IFilter<T> {
        private Func<T, bool> filter;

        public FuncFilter(Func<T, bool> filter) {
            this.filter = filter;
        }

        public bool Filter(T value) {
            return this.filter(value);
        }
    }
}