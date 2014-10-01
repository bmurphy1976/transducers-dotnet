using System;
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
}