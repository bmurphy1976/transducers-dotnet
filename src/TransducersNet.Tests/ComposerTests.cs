using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using NUnit.Framework;

namespace TransducersNet.Tests
{
    [TestFixture]
    public class ComposerTests
    {
        private ITransducer<string,int> ComposeUsingTransducers() 
        {
            return Composer
                .Compose(new FilterTransducer<int>(x => x % 3 == 0))
                .Compose(new MapTransducer<int,int>(x => x * x))
                .Compose(new MapTransducer<int,int>(x => x + 1))
                .Compose(new MapTransducer<int,string>(Convert.ToString))
                .Compose();
        }

        private ITransducer<string,int> ComposeUsingMapAndFilter()
        {
            return Composer
                .Filter<int>(x => x % 3 == 0)
                .Map<int>(x => x * x)
                .Map<int>(x => x + 1)
                .Map<string>(Convert.ToString)
                .Compose();
        }

        private IEnumerable<ITransducer<string,int>> GetComposedOperations()
        {
            yield return this.ComposeUsingTransducers();
            yield return this.ComposeUsingMapAndFilter();
        }

        private IEnumerable<int> GetInput()
        {
            return Enumerable.Range(0, 10);
        }

        private void AssertOutput(List<string> output)
        {
            Assert.AreEqual(4, output.Count);
            Assert.AreEqual("1", output[0]);
            Assert.AreEqual("10", output[1]);
            Assert.AreEqual("37", output[2]);
            Assert.AreEqual("82", output[3]);
        }

        [Test]
        public void TestTransduceWithListReducer() 
        {
            foreach (var xform in this.GetComposedOperations())
            {
                var reducer = new ListReducer<string>();
                var input = this.GetInput();
                var output = Transducer.Transduce(xform, reducer.Apply, reducer.Init(), input);

                this.AssertOutput(output);
            }
        }

        [Test]
        public void TestTransducerWithListAppender()
        {
            foreach (var xform in this.GetComposedOperations())
            {
                Func<List<string>, string, List<string>> apply = (result, x) => {
                    result.Add(x);
                    return result;
                };

                var input = this.GetInput();
                var output = Transducer.Transduce(xform, apply, new List<string>(), input);

                this.AssertOutput(output);
            }
        }

        [Test]
        public void TestTransducerWithIEnumerable()
        {
            foreach (var xform in this.GetComposedOperations())
            {
                var input = this.GetInput();
                var output = new List<string>();

                foreach (var x in Transducer.Enumerate(xform, input))
                {
                    output.Add(x);
                }

                this.AssertOutput(output);
            }
        }

        [Test]
        public void TestTransducerWithIObservable()
        {
            foreach (var xform in this.GetComposedOperations())
            {
                var input = this.GetInput();
                var output = new List<string>();

                var observable = Transducer.Observe(xform, input.ToObservable());

                observable.Subscribe(x => output.Add(x));

                this.AssertOutput(output);
            }
        }
    }
}