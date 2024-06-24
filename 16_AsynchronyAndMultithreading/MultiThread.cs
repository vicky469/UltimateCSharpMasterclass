using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;

namespace _16_AsynchronyAndMultithreading
{
    public class MultiThread
    {
        public static void Execute(string[] args)
        {
            System.Console.WriteLine("Cores count: " + Environment.ProcessorCount);
            System.Console.WriteLine("Main thread's ID: " + System.Threading.Thread.CurrentThread.ManagedThreadId);

            // Thread t1 = new Thread(() => PrintPluses(200));
            // Thread t2 = new Thread(() => PrintMinuses(200));
            // t1.Start();
            // t2.Start(); 

            var stopwatch = Stopwatch.StartNew();
            for(int i = 0 ; i < 100; i++)
            {
                //Thread newThread = new Thread(PrintA);
                //newThread.Start(); // ❌ time took: 14ms to create and start a new thread
                // or
                ThreadPool.QueueUserWorkItem(PrintA); // ✅ time took: 0ms to create and start a new thread
            }
            
            stopwatch.Stop();// once we reach this line, all threads can be created and started, not yet finished
            System.Console.WriteLine("Creation and starting time took: " + stopwatch.ElapsedMilliseconds);

        }
        static void PrintPluses(int count)
        {
            System.Console.WriteLine("PrintPluses thread's ID: " + System.Threading.Thread.CurrentThread.ManagedThreadId);
            for (int i = 0; i < count; i++)
            {
                Console.Write("+");
            }
        }
        static void PrintMinuses(int count)
        {
            System.Console.WriteLine("PrintMinuses thread's ID: " + System.Threading.Thread.CurrentThread.ManagedThreadId);
            for (int i = 0; i < count; i++)
            {
                Console.Write("-");
            }
        }

        public static void PrintA(object? obj)
        {
            Console.Write("A");
        }
    }
}