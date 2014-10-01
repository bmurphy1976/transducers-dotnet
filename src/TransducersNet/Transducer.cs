using System;
using System.Collections.Generic;

namespace TransducersNet
{
    public static class Transducer {
        public static IEnumerable<TOut> Enumerate<TIn,TOut>(ITransducer<TOut,TIn> transducer, IEnumerable<TIn> input) {
            var collector = new IdentityReducer<TOut>();
            var reducer = transducer.Transduce(collector);

            var result = collector.Init();
            var previous = result;

            foreach (var x in input) 
            {
                result = reducer.Apply(result, x);

                if (result != previous)
                    yield return result.Value;

                previous = result;
            }
        }

        public static IObservable<TOut> Observe<TIn,TOut>(ITransducer<TOut,TIn> transducer, IObservable<TIn> input) 
        {
            return new ObservableTransducer<TOut,TIn>(transducer, input);
        }

        public static R Transduce<TIn,TOut,R>(ITransducer<TOut,TIn> transducer, Func<R,TOut,R> apply, R init, IEnumerable<TIn> input) {
            var reducer = transducer.Transduce(new FuncReducer<TOut,R>(() => init, apply));

            var result = reducer.Init();

            foreach (var x in input) 
            {
                result = reducer.Apply(result, x);
            }

            return result;
        }
    }
}