using BenchmarkDotNet.Attributes;
using Perfolizer.Mathematics.SignificanceTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Benchmarking.Loops
{
    [SimpleJob(launchCount: 3, warmupCount: 10, targetCount: 30)]
    [MemoryDiagnoser(false)]
    [Orderer(BenchmarkDotNet.Order.SummaryOrderPolicy.FastestToSlowest)]
    public class BenchmarkLoop
    {
        [Params(100, 100_00)] 
        public int Size { get; set; }

        private int[] _items;

        [GlobalSetup] 
        public void Setup()
        {
            var random = new Random(Seed: 420);
            _items = Enumerable.Range(1, Size).Select(x => random.Next()).ToArray();
        }

        [Benchmark]
        public int[] For()
        {
            for(var i = 0; i < _items.Length; i++)
            {
                var item = _items[i];
                DoSomething(item);
            }
            return _items;
        }

        [Benchmark]
        public int[] Foreach()
        {
            foreach(var item in _items)
            {
                DoSomething(item);
            }
            return _items;
        }

        [Benchmark]
        public int[] For_Span()
        {
            Span<int> asSpan = _items;
            for(var i = 0;i < asSpan.Length; i++)
            {
                var item = asSpan[i];
                DoSomething(item);
            }

            return _items;
        }

        [Benchmark]
        public int[] Unsafe_For_Span_GetReference()
        {
            Span<int> asSpan = _items;
            ref var searchSpace = ref MemoryMarshal.GetReference(asSpan);

            for(var i = 0; i < asSpan.Length; i++)
            {
                var item = Unsafe.Add(ref searchSpace, i);
                DoSomething(item);
            }
            return _items;
        }

        private void DoSomething(int i)
        {
            //nuffin
        }
    }
}
