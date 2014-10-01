using System;

namespace TransducersNet
{
    public class IdentityReducer<T> : IReducer<T,Identity<T>> {
        public Identity<T> Init() {
            return null;
        }

        public Identity<T> Apply(T input) {
            return new Identity<T>(input);
        }

        public Identity<T> Apply(Identity<T> result, T input) {
            return new Identity<T>(input);
        }
    }
}