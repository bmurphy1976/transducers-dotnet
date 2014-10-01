using System;

namespace TransducersNet
{
    public interface IFilter<T> {
        bool Filter(T value);
    }
}
