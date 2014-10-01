Transducers for .NET
====================

This is an implementation of [Transducers](http://blog.cognitect.com/blog/2014/8/6/transducers-are-coming)
for the .NET platform written in C#.

The terminology used was chosen to try and convey the concepts to regular .NET
developers who aren't experts in type theory.

What are Transducers?
---------------------

Transducers are a technique that allows you to build and compose transformation
pipelines.  They are similar to Linq which works on IEnumerable and IObservable
collections, but they are "collection agnostic".  In a sense, theu are a more
generic form of Linq expressions that can work on targets of any type, not just
enumerable collections.

They could be applied to state machines, regular objects, channels, or any number
of other abstractions.  Watch [this](https://www.youtube.com/watch?v=6mTbuzafcII)
presentation by Rich Hickey for a detailed explanation and examples.

The neat thing about them is they don't require any special functionality or
knowledge to use.  You don't have to be an expert on Monads or Type Theory to use
or understand them.  They are merely an application of basic functional composition.

What do they look like?
-----------------------

The following code:

```
// Append a value to a list and return the updated list
Func<List<string>, string, List<string>> appender = (result, value) => {
    result.Add(value);
    return result;
};

var filter = new FilterTransducer<int>(x => x % 3 == 0);
var square = new MapTransducer<int,int>(x => x * x);
var addOne = new MapTransducer<int,int>(x => x + 1);
var toString = new MapTransducer<int,string>(Convert.ToString);

var xform = Composer
    .Compose(filter)
    .Compose(square)
    .Compose(addOne)
    .Compose(toString)
    .Compose();

var input = Enumerable.Range(0,10);
var output = Transducer.Transduce(xform, appender, new List<string>(), input);

foreach (var x in output)
{
    Console.Out.WriteLine(x);
}
```

Outputs:

```
1
10
37
82
```

With a little syntactic sugar, the transformation pipeline can be condensed to
this:

```
var xform = Composer<int>
    .Filter(x => x % 3 == 0)
    .Map(x => x * x)
    .Map(x => x + 1)
    .Map(Convert.ToString)
    .Compose();

var input = Enumerable.Range(0, 10);
var output = Transducer.ToList(xform, input);

foreach (var x in output)
{
    Console.Out.WriteLine(x);
}
```

License
=======

The MIT License (MIT)

Copyright (c) [2014] [Bryan Murphy]

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
