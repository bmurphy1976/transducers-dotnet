using System;

namespace TransducersNet
{
    public class Identity<T> {
        public Identity(T value) {
            this.Value = value;
        }

        public T Value {
            get;
            private set;
        }
    }
}