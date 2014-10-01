using System;

namespace TransducersNet
{
    public class MapTransducer<TIn,TOut> : ITransducer<TOut,TIn> {
        private IMap<TIn,TOut> map;

        public MapTransducer(Func<TIn,TOut> map)
            : this(new FuncMap<TIn,TOut>(map)) {
        }

        public MapTransducer(IMap<TIn,TOut> map) {
            this.map = map;
        }

        public IReducer<TIn,R> Transduce<R>(IReducer<TOut,R> reducer) {
            return new FuncReducer<TIn,R>(
                reducer.Init,
                (R result, TIn value) => {
                return reducer.Apply(result, this.map.Map(value));
            });
        }
    }
}