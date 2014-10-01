using System;
using System.Collections.Generic;

namespace TransducersNet
{
    public class ListReducer<T> : IReducer<T, List<T>> {
        public List<T> Init() {
            return new List<T>();
        }

        public List<T> Apply(T input) {
            return this.Apply(this.Init(), input);
        }

        public List<T> Apply(List<T> result, T input) {
            result.Add(input);
            return result;
        }
    }
}