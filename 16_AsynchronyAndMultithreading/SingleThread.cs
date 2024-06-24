using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _16_AsynchronyAndMultithreading
{
    public class SingleThread
    {
         public static void Execute(string[] args)
        {
            System.Console.WriteLine("Cores count: " + Environment.ProcessorCount);
            System.Console.WriteLine("Main thread's ID: " + System.Threading.Thread.CurrentThread.ManagedThreadId);
            PrintPluses(10);
            PrintMinuses(10);
        }

        public static void PrintPluses(int count)
        {
            System.Console.WriteLine("PrintPluses thread's ID: " + System.Threading.Thread.CurrentThread.ManagedThreadId);
            for (int i = 0; i < count; i++)
            {
                Console.Write("+");
            }
        }
        public static void PrintMinuses(int count)
        {
            System.Console.WriteLine("PrintMinuses thread's ID: " + System.Threading.Thread.CurrentThread.ManagedThreadId);
            for (int i = 0; i < count; i++)
            {
                Console.Write("-");
            }
        }

    }
}