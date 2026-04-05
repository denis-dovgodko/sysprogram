using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ParallelLoopExitDemo
{
    public class Template
    {
        const double TARGET = 52;
        const double EPSILON = 0.1;
        static void Main(string[] args) {}
        public static void RunWithBreak()
        {
            Console.WriteLine("using break()");

            int size = 100;
            double[] data = new double[size];
            var finishedIndices = new ConcurrentBag<int>();

            for (int i = 0; i < size; i++)
                data[i] = i;
                

            Stopwatch sw = Stopwatch.StartNew();

            ParallelLoopResult result = Parallel.For(0, size, (i, state) =>
            {
                var rand = new Random(i * DateTime.Now.Millisecond);
                if (Math.Abs(data[i] - TARGET) < EPSILON)
                {
                    Console.WriteLine($"[Break] Found value {data[i]} at index {i}");
                    state.Break();
                }
                Thread.Sleep(rand.Next(1, 4));
                finishedIndices.Add(i);
            });

            sw.Stop();

            var sorted = finishedIndices.ToArray();
            Array.Sort(sorted);

            Console.WriteLine("Finished indices: " + string.Join(", ", sorted));

            if (!result.IsCompleted)
                Console.WriteLine($"Loop stopped at iteration {result.LowestBreakIteration}");

            Console.WriteLine($"Time: {sw.ElapsedMilliseconds} ms");
        }

        public static void RunWithStop()
        {
            Console.WriteLine("Using stop()");

            int size = 100;
            double[] data = new double[size];
            var finishedIndices = new ConcurrentBag<int>();

            for (int i = 0; i < size; i++)
                data[i] = i;

            Stopwatch sw = Stopwatch.StartNew();

            ParallelLoopResult result = Parallel.For(0, size, (i, state) =>
            {
                var rand = new Random(i * DateTime.Now.Millisecond);
                if (Math.Abs(data[i] - TARGET) < EPSILON)
                {
                    Console.WriteLine($"[Stop] Found value {data[i]} at index {i}");
                    state.Stop();
                }
                Thread.Sleep(rand.Next(1, 4));
                finishedIndices.Add(i);
            });

            sw.Stop();

            var sorted = finishedIndices.ToArray();
            Array.Sort(sorted);

            Console.WriteLine("Finished indices: " + string.Join(", ", sorted));

            if (!result.IsCompleted)
                Console.WriteLine("Loop was stopped early");

            Console.WriteLine($"Time: {sw.ElapsedMilliseconds} ms");
        }
        public static void ParallelForEachExample(int size, int target, bool useBreak)
        {
            var numbers = Enumerable.Range(0, size).ToList();
            var finishedIndices = new ConcurrentBag<int>();

            Parallel.ForEach(numbers, ParallelBody);

            void ParallelBody(int num, ParallelLoopState state)
            {
                var rand = new Random(num * DateTime.Now.Millisecond);

                if (num == target)
                {
                    if (useBreak)
                        state.Break();
                    else
                        state.Stop();
                }

                Thread.Sleep(rand.Next(1, 4));
                finishedIndices.Add(num);
            }

            var sorted = finishedIndices.ToArray();
            Array.Sort(sorted);

            Console.WriteLine(useBreak ? "[Break] Finished indices: " : "[Stop] Finished indices: ");
            Console.WriteLine(string.Join(", ", sorted));
        }
    }
}