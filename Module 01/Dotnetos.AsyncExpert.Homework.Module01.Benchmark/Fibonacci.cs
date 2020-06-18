﻿using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using BenchmarkDotNet.Attributes;

namespace Dotnetos.AsyncExpert.Homework.Module01.Benchmark
{
    [DisassemblyDiagnoser(exportCombinedDisassemblyReport: true)]
    public class FibonacciCalc
    {
        // HOMEWORK:
        // 1. Write implementations for RecursiveWithMemoization and Iterative solutions
        // 2. Add memory profiler to the benchmark
        // 3. Run with release configuration and compare results
        // 4. Open disassembler report and compare machine code
        // 
        // You can use the discussion panel to compare your results with other students

        [Benchmark(Baseline = true)]
        [ArgumentsSource(nameof(Data))]
        public ulong Recursive(ulong n)
        {
            if (n == 1 || n == 2) return 1;
            return Recursive(n - 2) + Recursive(n - 1);
        }

        private readonly Dictionary<ulong, ulong> _dictionary = new Dictionary<ulong, ulong>();

        [Benchmark]
        [ArgumentsSource(nameof(Data))]
        public ulong RecursiveWithMemoization(ulong n)
        {            
            if (n == 1 || n == 2) return 1;
            if (!_dictionary.TryGetValue(n, out ulong value))
            {
                value = RecursiveWithMemoization(n - 1) + RecursiveWithMemoization(n - 2);
                _dictionary.Add(n, value);
            }
            return value;
        }

        [Benchmark]
        [ArgumentsSource(nameof(Data))]
        public ulong Iterative(ulong n)
        {
            ulong current = 0, prev1, prev2;
            prev1 = prev2 = 1;

            if (n == 1 || n == 2) return 1;

            for (ulong i = 2; i < n; i++)
            {
                current = prev1 + prev2;
                prev2 = prev1;
                prev1 = current;
            }

            return current;
        }

        public IEnumerable<ulong> Data()
        {
            yield return 15;
            yield return 35;
        }
    }
}
