using System;

namespace TransducersNet
{
    public class FuncMap<TIn,TOut> : IMap<TIn,TOut> {
        private Func<TIn,TOut> map;

        public FuncMap(Func<TIn,TOut> map) {
            this.map = map;
        }

        public TOut Map(TIn value) {
            return this.map(value);
        }
    }
}