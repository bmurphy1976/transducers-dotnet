using System;

namespace TransducersNet
{
    public class FuncReducer<T, R> : IReducer<T, R> {
        private Func<R> init;
        private Func<R, T, R> reduce;

        public FuncReducer(Func<R> init, Func<R, T, R> reduce) {
            this.init = init;
            this.reduce = reduce;
        }

        public R Init() {
            return this.init();
        }

        public R Apply(T input) {
            return this.reduce(this.Init(), input);
        }

        public R Apply(R result, T input) {
            return this.reduce(result, input);
        }
    }
}