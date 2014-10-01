using System;

namespace TransducersNet
{
    public class FilterTransducer<T> : ITransducer<T,T> {
        private IFilter<T> filter;

        public FilterTransducer(Func<T, bool> filter)
            : this(new FuncFilter<T>(filter)) {
        }

        public FilterTransducer(IFilter<T> filter) {
            this.filter = filter;
        }

        public IReducer<T, R> Transduce<R>(IReducer<T, R> reducer) {
            return new FuncReducer<T, R>(
                reducer.Init,
                (R result, T value) => {
                if (this.filter.Filter(value)) {
                    return reducer.Apply(result, value);
                } else {
                    return result;
                }
            });
        }
    }
}