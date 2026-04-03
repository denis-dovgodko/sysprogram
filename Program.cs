using System;
using System.Threading;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace sysprogram
{
    internal class Program
    {
        static void PrintNumbers()
        {
            Thread.CurrentThread.Name = "NumbersThread";
            for (int i = 1; i <= 40; i++)
            {
                Console.WriteLine($"{Thread.CurrentThread.Name}: {i}");
                Thread.Sleep(200);
            }
        }

        static void PrintLetters()
        {
            Thread.CurrentThread.Name = "LettersThread";
            for (char c = 'A'; c <= 'Z'; c++)
            {
                Console.WriteLine($"{Thread.CurrentThread.Name}: {c}");
                Thread.Sleep(300);
            }
        }

        static void RunTask1()
        {
            var t1 = new Thread(PrintNumbers);
            var t2 = new Thread(PrintLetters);

            t1.Start();
            t2.Start();

            t1.Join();
            t2.Join();
        }

        static void PriorityWork()
        {
            Console.WriteLine($"{Thread.CurrentThread.Name} started");
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine($"{Thread.CurrentThread.Name}: {i}");
                Thread.Sleep(200);
            }
            Console.WriteLine($"{Thread.CurrentThread.Name} finished");
        }

        static void BackgroundWork()
        {
            while (true)
            {
                Console.WriteLine("Background thread working...");
                Thread.Sleep(500);
            }
        }

        static void RunTask2()
        {
            var t1 = new Thread(PriorityWork) { Name = "P1", Priority = ThreadPriority.AboveNormal };
            var t2 = new Thread(PriorityWork) { Name = "P2", Priority = ThreadPriority.Highest };
            var bg = new Thread(BackgroundWork) { IsBackground = true };

            t1.Start();
            t2.Start();
            bg.Start();

            t1.Join();
            t2.Join();
        }

        const int LIMIT = 100_000_000;
        static long[] results3 = new long[5];

        static void Worker3(object obj)
        {
            int index = (int)obj;

            while (!stop)
            {
                results3[index]++;
            }
        }

        static void RunTask3()
        {
            ThreadPriority[] priorities =
            {
                ThreadPriority.Highest,
                ThreadPriority.Lowest,
                ThreadPriority.AboveNormal,
                ThreadPriority.Normal,
                ThreadPriority.BelowNormal
            };

            Thread[] threads = new Thread[5];
            results3 = new long[5];
            stop = false;

            Process.GetCurrentProcess().ProcessorAffinity = (IntPtr)1;

            for (int i = 0; i < 5; i++)
            {
                int localI = i;

                threads[i] = new Thread(Worker3);
                threads[i].Priority = priorities[i];

                threads[i].Start(localI);
            }

            Thread.Sleep(5000);

            stop = true;
            foreach (var t in threads)
                t.Join();

            long total = 0;
            foreach (var r in results3)
                total += r;

            Console.WriteLine("\nTask 3 results:");
            for (int i = 0; i < 5; i++)
            {
                double p = total == 0 ? 0 : (double)results3[i] / total * 100;
                Console.WriteLine($"{i}: {priorities[i]} | {p:F2}%");
            }
        }

        class WorkerInfo
        {
            public int Id;
            public Thread Thread;
            public long Counter;
            public ThreadPriority Priority;
            public bool Running = true;
        }

        static volatile bool stop = false;
        static List<WorkerInfo> workers = new List<WorkerInfo>();

        static void Worker4(object obj)
        {
            var w = (WorkerInfo)obj;

            while (!stop)
                w.Counter++;

            w.Running = false;
        }

        static ThreadPriority ParsePriority(string s)
        {
            switch (s.ToLower())
            {
                case "lowest": return ThreadPriority.Lowest;
                case "below": return ThreadPriority.BelowNormal;
                case "normal": return ThreadPriority.Normal;
                case "above": return ThreadPriority.AboveNormal;
                case "highest": return ThreadPriority.Highest;
                default: return ThreadPriority.Normal;
            }
        }

        static void RunTask4()
        {
            Console.Write("Threads count: ");
            int n = int.Parse(Console.ReadLine());

            workers.Clear();
            stop = false;

            for (int i = 0; i < n; i++)
            {
                Console.Write($"Thread {i} priority (lowest/below/normal/above/highest): ");
                var p = ParsePriority(Console.ReadLine());

                var w = new WorkerInfo
                {
                    Id = i,
                    Priority = p,
                    Counter = 0,
                    Running = false
                };

                w.Thread = new Thread(Worker4);
                w.Thread.Priority = p;

                workers.Add(w);
            }

            foreach (var w in workers)
                w.Thread.Start(w);

            var monitor = new Thread(() =>
            {
                while (!stop)
                {
                    Console.Clear();
                    Console.WriteLine("Live:\n");

                    foreach (var w in workers)
                    {
                        Console.WriteLine(
                            $"T{w.Id} | {w.Priority} | {w.Counter} | {(w.Running ? "Run" : "Done")}");
                    }

                    Thread.Sleep(500);
                }
            });

            monitor.IsBackground = true;
            monitor.Start();

            Console.WriteLine("\nPress ENTER to stop...");
            Console.ReadLine();

            stop = true;

            foreach (var w in workers)
                w.Thread.Join();

            long total = workers.Sum(w => w.Counter);

            Console.Clear();
            Console.WriteLine("Result:\n");

            foreach (var w in workers)
            {
                double percent = total == 0 ? 0 : (double)w.Counter / total * 100;
                Console.WriteLine($"T{w.Id} | {w.Priority} | {percent:F2}%");
            }
        }

        static void Main()
        {
            while (true)
            {
                Console.WriteLine("\n--- Main Menu ---");
                Console.WriteLine("1 - Run Task 1 (Numbers & Letters)");
                Console.WriteLine("2 - Run Task 2 (Priority & Background)");
                Console.WriteLine("3 - Run Task 3 (Static Priority Test)");
                Console.WriteLine("4 - Run Task 4 (Interactive Thread Monitor)");
                Console.WriteLine("0 - Exit");
                Console.Write("\nSelect task: ");

                var input = Console.ReadLine();

                if (input == "0")
                {
                    Console.WriteLine("Exiting...");
                    break;
                }

                switch (input)
                {
                    case "1":
                        RunTask1();
                        break;
                    case "2":
                        RunTask2();
                        break;
                    case "3":
                        RunTask3();
                        break;
                    case "4":
                        RunTask4();
                        break;
                    default:
                        Console.WriteLine("Invalid selection, try again.");
                        break;
                }

                Console.WriteLine("\nTask finished. Press Enter to return to menu...");
                Console.ReadLine();
            }
        }
    }
}