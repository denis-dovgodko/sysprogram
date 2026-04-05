using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Linq;
using ParallelLoopExitDemo;
using ParallelLambdaLoopsDemo;

namespace ParallelLoopsDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Select command to investigate Parallel:");
                Console.WriteLine("1 - Parallel.For() experiments with different array types and sizes");
                Console.WriteLine("2 - Parallel.For() with exit condition");
                Console.WriteLine("3 - Parallel.ForEach()");
                Console.WriteLine("4 - Parallel.ForEach() with lambda expression");
                Console.WriteLine("0 - Exit");
                Console.Write("Your choice: ");
                var key = Console.ReadLine();

                switch (key)
                {
                    case "1":
                        ParallelForExperiments();
                        break;
                    case "2":
                        Console.WriteLine("Parallel.For() with exit condition demo\n");
                        ParallelLoopExitDemo.Template.RunWithBreak();
                        Console.WriteLine("\nnext\n");
                        ParallelLoopExitDemo.Template.RunWithStop();
                        break;
                    case "3":
                        Console.WriteLine("Parallel.ForEach() with exit condition demo\n");
                        ParallelLoopExitDemo.Template.ParallelForEachExample(100, 50, true);
                        Console.WriteLine("\nnext\n");
                        ParallelLoopExitDemo.Template.ParallelForEachExample(100, 50, false);
                        break;
                    case "4":
                        Console.WriteLine("Parallel.ForEach() with exit condition demo\n");
                        ParallelLambdaLoopsDemo.Template.ParallelForEachLambdaExample(100, 50, true);
                        Console.WriteLine("\nnext\n");
                        ParallelLambdaLoopsDemo.Template.ParallelForEachLambdaExample(100, 50, false);
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Wrong option");
                        break;
                }

                Console.WriteLine("Press enter to continue");
                Console.ReadLine();
            }
        }

        static void ParallelForExperiments()
        {
            int[] sizes = { 100_000, 1_000_000 };
            string[] formulas = { "x/10", "x/pi", "e^x/x^pi", "e^(pi*x)/x^pi"};

            Console.WriteLine("Type\tSize\tFormula\tSerial(s)\tParallel(s)\tSpeedUp");

            foreach (var size in sizes)
            {
                double[] dataDouble = new double[size];
                int[] dataInt = new int[size];
                for (int i = 0; i < size; i++)
                {
                    dataDouble[i] = i + 1;
                    dataInt[i] = i + 1;
                }

                foreach (var formula in formulas)
                {
                    Stopwatch sw = new Stopwatch();
                    sw.Restart();
                    for (int i = 0; i < dataDouble.Length; i++)
                        dataDouble[i] = ComputeDouble(dataDouble[i], formula);
                    sw.Stop();
                    double serialTime = sw.Elapsed.TotalSeconds;

                    sw.Restart();
                    Parallel.For(0, dataDouble.Length, i =>
                    {
                        dataDouble[i] = ComputeDouble(dataDouble[i], formula);
                    });
                    sw.Stop();
                    double parallelTime = sw.Elapsed.TotalSeconds;

                    double speedUp = serialTime / parallelTime;
                    Console.WriteLine($"double\t{size}\t{formula}\t{serialTime:F4}\t\t{parallelTime:F4}\t\t{speedUp:F2}");

                    sw.Restart();
                    for (int i = 0; i < dataInt.Length; i++)
                        dataInt[i] = ComputeInt(dataInt[i], formula);
                    sw.Stop();
                    serialTime = sw.Elapsed.TotalSeconds;

                    sw.Restart();
                    Parallel.For(0, dataInt.Length, i =>
                    {
                        dataInt[i] = ComputeInt(dataInt[i], formula);
                    });
                    sw.Stop();
                    parallelTime = sw.Elapsed.TotalSeconds;

                    speedUp = serialTime / parallelTime;
                    Console.WriteLine($"int\t{size}\t{formula}\t{serialTime:F4}\t\t{parallelTime:F4}\t\t{speedUp:F2}");
                }
            }
        }

        static double ComputeDouble(double x, string formula)
        {
            switch (formula)
            {
                case "x/10": return x / 10;
                case "x/pi": return x / Math.PI;
                case "e^x/x^pi": return Math.Exp(x) / Math.Pow(x, Math.PI);
                case "e^(pi*x)/x^pi": return Math.Exp(Math.PI * x) / Math.Pow(x, Math.PI);
                default: return x;
            }
        }

        static int ComputeInt(int x, string formula)
        {
            switch (formula)
            {
                case "x/10": return x / 10;
                case "x/pi": return (int)(x / Math.PI);
                case "e^x/x^pi": return (int)(Math.Exp(x) / Math.Pow(x, Math.PI));
                case "e^(pi*x)/x^pi": return (int)(Math.Exp(Math.PI * x) / Math.Pow(x, Math.PI));
                default: return x;
            }
        }

        static void ParallelForBreakExample()
        {
            int size = 1_000_000;
            double[] data = new double[size];
            for (int i = 0; i < size; i++) data[i] = i;
            data[500_000] = -1;

            ParallelLoopResult result = Parallel.For(0, size, (i, state) =>
            {
                if (data[i] < 0) state.Break();
            });

            if (!result.IsCompleted)
                Console.WriteLine($"Loop was interrupted on {result.LowestBreakIteration} iteration");
            else
                Console.WriteLine("Loop finished completely");
        }

        static void ParallelForEachExample()
        {
            int[] numbers = Enumerable.Range(1, 20).ToArray();

            Parallel.ForEach(numbers, n =>
            {
                Console.WriteLine($"Processing number: {n}");
            });
        }

        static void ParallelForEachLambda()
        {
            int[] numbers = Enumerable.Range(1, 20).ToArray();

            Parallel.ForEach(numbers, (n, state) =>
            {
                Console.WriteLine($"Processing with lambda: {n}");
                if (n == 15) state.Break();
            });
        }
    }
}