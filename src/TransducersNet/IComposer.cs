using System;

namespace TransducersNet
{
    public interface IComposer<TOut,TIn> {
        IComposer<TNext,TIn> Map<TNext>(Func<TOut,TNext> f);
        IComposer<TOut,TIn> Filter(Func<TOut,bool> f);

        IComposer<TNext,TIn> Compose<TNext>(ITransducer<TNext,TOut> transducer);
        ITransducer<TOut,TIn> Compose();

        IReducer<TIn,R> Transduce<R>(IReducer<TOut,R> reducer);
    }
}    