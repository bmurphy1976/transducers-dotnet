using System;

namespace TransducersNet
{
    public interface IReducer<T,R> {
        R Init();
        R Apply(T input);
        R Apply(R result, T input);
    }
}