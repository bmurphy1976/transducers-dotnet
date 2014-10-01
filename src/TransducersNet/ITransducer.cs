using System;

namespace TransducersNet
{
    public interface ITransducer<TOut,TIn> {
        IReducer<TIn, R> Transduce<R>(IReducer<TOut,R> reducer);
    }
}