using System;
using System.Threading;
using System.Threading.Tasks;

namespace TPL_Lab
{
    internal class Program
    {
        static void NumbersTask()
        {
            for (int i = 1; i <= 10; i++)
            {
                Console.WriteLine(i);
                Task.Delay(200).Wait();
            }
        }

        static void LettersTask()
        {
            for (char c = 'A'; c <= 'J'; c++)
            {
                Console.WriteLine(c);
                Task.Delay(200).Wait();
            }
        }

        static void RunTask1()
        {
            Console.WriteLine("Task 1: Numbers & Letters");
            Task t1 = new Task(NumbersTask);
            Task t2 = new Task(LettersTask);

            t1.Start();
            t2.Start();

            Task.WaitAll(t1, t2);
            Console.WriteLine("Task 1 finished.");
        }

        static void CountTask()
        {
            Console.WriteLine($"Task {Task.CurrentId} started.");
            for (int i = 1; i <= 5; i++)
            {
                Console.WriteLine($"Task {Task.CurrentId} counter = {i}");
                Task.Delay(200).Wait();
            }
            Console.WriteLine($"Task {Task.CurrentId} finished.");
        }

        static void RunTask2()
        {
            Console.WriteLine("Task 2: Id & CurrentId demonstration");
            Task t1 = Task.Factory.StartNew(CountTask);
            Task t2 = Task.Factory.StartNew(CountTask);
            Task t3 = Task.Factory.StartNew(CountTask);

            Console.WriteLine($"Id of t1 = {t1.Id}");
            Console.WriteLine($"Id of t2 = {t2.Id}");
            Console.WriteLine($"Id of t3 = {t3.Id}");

            Task.WaitAll(t1, t2, t3);
            Console.WriteLine("Task 2 finished.");
        }

        static int SumTask(object n)
        {
            int sum = 0;
            for (int i = 1; i <= (int)n; i++)
                sum += i;
            return sum;
        }

        static void RunTask3()
        {
            Console.Write("Enter N: ");
            int N = int.Parse(Console.ReadLine());

            Task<int> t1 = Task.Factory.StartNew(SumTask, N);

            Task t2 = t1.ContinueWith(task =>
            {
                Console.WriteLine($"Sum from 1 to {N} = {task.Result}");
            });

            t2.Wait();
            Console.WriteLine("Task 3 finished.");
        }

        static void FactorialTask()
        {
            int n = 5;
            long fact = 1;
            for (int i = 1; i <= n; i++)
                fact *= i;
            Console.WriteLine($"[Thread {Thread.CurrentThread.ManagedThreadId}]: Factorial of {n} = {fact}");
        }

        static void SumTaskNoArg()
        {
            int n = 5;
            int sum = 0;
            for (int i = 1; i <= n; i++)
                sum += i;
            Console.WriteLine($"[Thread {Thread.CurrentThread.ManagedThreadId}]: Sum from 1 to {n} = {sum}");
        }

        static void MessageTask()
        {
            for (int i = 0; i < 5; i++)
            {
                Console.WriteLine($"[Thread {Thread.CurrentThread.ManagedThreadId}]: Message from Task 4");
                Task.Delay(300).Wait();
            }
        }

        static void RunTask4()
        {
            Console.WriteLine("Task 4: Parallel.Invoke demonstration");
            Parallel.Invoke(FactorialTask, SumTaskNoArg, MessageTask);
            Console.WriteLine("Task 4 finished.");
        }

        static void Main()
        {
            while (true)
            {
                Console.WriteLine("\n--- Main Menu ---");
                Console.WriteLine("1 - Run Task 1 (Numbers & Letters)");
                Console.WriteLine("2 - Run Task 2 (Id & CurrentId)");
                Console.WriteLine("3 - Run Task 3 (Sum & ContinueWith)");
                Console.WriteLine("4 - Run Task 4 (Parallel.Invoke demo)");
                Console.WriteLine("0 - Exit");
                Console.Write("Select task: ");

                var input = Console.ReadLine();
                switch (input)
                {
                    case "1": RunTask1(); break;
                    case "2": RunTask2(); break;
                    case "3": RunTask3(); break;
                    case "4": RunTask4(); break;
                    case "0": Console.WriteLine("Exiting..."); return;
                    default: Console.WriteLine("Invalid selection."); break;
                }

                Console.WriteLine("\nPress Enter to return to menu...");
                Console.ReadLine();
            }
        }
    }
}