using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _16_AsynchronyAndMultithreading
{
    public class AsyncExceptionHandling
    {
        public static async void Execute(string[] args)
        {
            // ❌ Problem: exception is not caught because it is thrown in a different thread
            // try
            // {
            //     var task = new Thread(() => MethodThrowingException());
            //     task.Start();
            // }
            // catch (Exception ex) // try to catch in the main thread
            // {
            //     Console.WriteLine("Exception: " + ex.InnerException.Message);
            // }

            // ✅ Solution: throw the exception within the task
            try
            {
                var task = Task.Run(() => MethodThrowingException());
                await task;
            }
            catch (AggregateException ex) 
            {
                Console.WriteLine("Exception: " + ex.InnerException.Message);
            }
            
        }

        static void MethodThrowingException()
        {
            System.Console.WriteLine("MethodThrowingException thread's ID: " + System.Threading.Thread.CurrentThread.ManagedThreadId);
            throw new Exception("Exception from a thread");
        }
    }
}