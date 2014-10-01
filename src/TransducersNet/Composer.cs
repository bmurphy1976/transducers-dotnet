using System;

namespace TransducersNet
{
    public static class Composer {
        public static Composer<T,T> Compose<T,T>(ITransducer<T,T> transducer) {
            return new Composer<T,T>(transducer);
        }

        public static Composer<TOut,TIn> Compose<TOut,TIn>(ITransducer<TOut,TIn> transducer) {
            return new Composer<TOut,TIn>(transducer);
        }

        public static Composer<TOut,TIn> Map<TOut,TIn>(Func<TIn,TOut> f) {
            return new Composer<TOut,TIn>(new MapTransducer<TIn,TOut>(f));
        }

        public static IComposer<T,T> Filter<T>(Func<T,bool> f) {
            return new Composer<T,T>(new FilterTransducer<T>(f));
        }
    }

    public class Composer<TOut,TIn> : IComposer<TOut,TIn> {
        private ITransducer<TOut,TIn> transducer;

        public Composer(ITransducer<TOut,TIn> transducer) {
            this.transducer = transducer;
        }

        public IComposer<TFloat,TIn> Compose<TFloat>(ITransducer<TFloat,TOut> transducer) {
            return new Composer<TFloat,TOut,TIn>(this, transducer);
        }

        public IComposer<TNext,TIn> Map<TNext>(Func<TOut,TNext> f) {
            return new Composer<TNext,TOut,TIn>(this, new MapTransducer<TOut,TNext>(f));
        }

        public IComposer<TOut,TIn> Filter(Func<TOut,bool> f) {
            return new Composer<TOut,TOut,TIn>(this, new FilterTransducer<TOut>(f));
        }

        public ITransducer<TOut,TIn> Compose() {
            return new ComposerTransducer<TOut,TIn>(this);
        }

        public IReducer<TIn,R> Transduce<R>(IReducer<TOut,R> reducer) {
            return this.transducer.Transduce(reducer);
        }
    }

    public class Composer<TOut, TIntermediate, TIn> : IComposer<TOut,TIn> {
        private IComposer<TIntermediate,TIn> composer;
        private ITransducer<TOut,TIntermediate> transducer;

        public Composer(IComposer<TIntermediate,TIn> composer, ITransducer<TOut,TIntermediate> transducer) {
            this.composer = composer;
            this.transducer = transducer;
        }

        public IComposer<TNext,TIn> Compose<TNext>(ITransducer<TNext,TOut> transducer) {
            return new Composer<TNext,TOut,TIn>(this, transducer);
        }

        public IComposer<TNext,TIn> Map<TNext>(Func<TOut,TNext> f) {
            return new Composer<TNext,TOut,TIn>(this, new MapTransducer<TOut,TNext>(f));
        }

        public IComposer<TOut,TIn> Filter(Func<TOut,bool> f) {
            return new Composer<TOut,TOut,TIn>(this, new FilterTransducer<TOut>(f));
        }

        public ITransducer<TOut,TIn> Compose() {
            return new ComposerTransducer<TOut,TIn>(this);
        }
 
        public IReducer<TIn,R> Transduce<R>(IReducer<TOut,R> reducer) {
            return this.composer.Transduce(this.transducer.Transduce(reducer));
        }
    }
}