using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ParallelLoopExitDemo
{
    class Program
    {
        const double TARGET = 500_000;
        const double EPSILON = 0.1;

        static void Main(string[] args)
        {
            Console.WriteLine("Parallel.For() with exit condition demo\n");

            RunWithBreak();
            Console.WriteLine("\nnext\n");
            RunWithStop();

            Console.WriteLine("\npress enter to exit");
            Console.ReadLine();
        }

        static void RunWithBreak()
        {
            Console.WriteLine("using break()");

            int size = 1_000_000;
            double[] data = new double[size];

            for (int i = 0; i < size; i++)
                data[i] = i;

            Stopwatch sw = Stopwatch.StartNew();

            ParallelLoopResult result = Parallel.For(0, size, (i, state) =>
            {
                if (Math.Abs(data[i] - TARGET) < EPSILON)
                {
                    Console.WriteLine($"[Break] Found value {data[i]} at index {i}");
                    state.Break();
                }
            });

            sw.Stop();

            if (!result.IsCompleted)
                Console.WriteLine($"Loop stopped at iteration {result.LowestBreakIteration}");

            Console.WriteLine($"Time: {sw.ElapsedMilliseconds} ms");
        }

        static void RunWithStop()
        {
            Console.WriteLine("Using stop()");

            int size = 1_000_000;
            double[] data = new double[size];

            for (int i = 0; i < size; i++)
                data[i] = i;

            Stopwatch sw = Stopwatch.StartNew();

            ParallelLoopResult result = Parallel.For(0, size, (i, state) =>
            {
                if (Math.Abs(data[i] - TARGET) < EPSILON)
                {
                    Console.WriteLine($"[Stop] Found value {data[i]} at index {i}");
                    state.Stop();
                }
            });

            sw.Stop();

            if (!result.IsCompleted)
                Console.WriteLine("Loop was stopped early");

            Console.WriteLine($"Time: {sw.ElapsedMilliseconds} ms");
        }
    }
}