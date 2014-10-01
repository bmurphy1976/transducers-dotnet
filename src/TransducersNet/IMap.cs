using System;

namespace TransducersNet
{
    public interface IMap<TIn,TOut> {
        TOut Map(TIn value);
    }
}