using System;

namespace TransducersNet
{
    internal class ComposerTransducer<TOut,TIn> : ITransducer<TOut,TIn> {
        private IComposer<TOut,TIn> composer;

        public ComposerTransducer(IComposer<TOut,TIn> composer) {
            this.composer = composer;
        }

        public IReducer<TIn, R> Transduce<R>(IReducer<TOut,R> reducer) {
            return this.composer.Transduce(reducer);
        }
    }
}