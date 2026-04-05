using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ParallelLambdaLoopsDemo
{
    public class Template
    {
        static void Main(string[] args)
        {
        }
        public static void ParallelForEachLambdaExample(int size, int target, bool useBreak)
        {
            var numbers = Enumerable.Range(0, size).ToList();
            var finishedIndices = new ConcurrentBag<int>();

            Parallel.ForEach(numbers, (num, state) =>
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
            });

            var sorted = finishedIndices.ToArray();
            Array.Sort(sorted);

            Console.WriteLine(useBreak ? "[Break] Finished indices: " : "[Stop] Finished indices: ");
            Console.WriteLine(string.Join(", ", sorted));
        }
    }
}
