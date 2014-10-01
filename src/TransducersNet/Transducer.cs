using System;
using System.Collections.Generic;
using System.Reactive;

namespace TransducersNet
{
    public class ObservableTransducer<TOut,TIn> : IObservable<TOut>
    {
        private ITransducer<TOut,TIn> transducer;
        private IObservable<TIn> input;

        public ObservableTransducer(ITransducer<TOut,TIn> transducer, IObservable<TIn> input)
        {
            this.transducer = transducer;
            this.input = input;
        }

        public IDisposable Subscribe(IObserver<TOut> observer)
        {
            var collector = new IdentityReducer<TOut>();
            var reducer = transducer.Transduce(collector);

            var result = collector.Init();
            var previous = result;

            this.input.Subscribe(x => {
                result = reducer.Apply(result, x);

                if (result != previous)
                    observer.OnNext(result.Value);

                previous = result;
            });

            observer.OnCompleted();

            return null;
        }
    }

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