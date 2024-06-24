using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _16_AsynchronyAndMultithreading
{
    public class Tpl
    { 
        public static void Execute(string[] args)
        {
            #region 1) Task return void
            //    Step 1: Start the tasks using Task.Run
            //      Task task1 = Task.Run(() => PrintPluses(200));
            //      Task task2 = Task.Run(() => PrintMinuses(200));

            //    Step 2: Wait for both tasks to complete
            //      Task.WaitAll(task1, task2);
            #endregion
            #region 2) Task return value

            // Task<int> task = Task.Run(() => CalculateLen("Hello World"));
            // System.Console.WriteLine("Length is: " + task.Result); // ❌ this will block the main thread until the task is completed

            // string userInput;
            // do{
            //     System.Console.WriteLine("Enter a command:");
            //     userInput = Console.ReadLine();
            // }while(userInput != "exit");  // we can't enter a command until the task is completed

            #endregion
            #region 3) Task return value with Wait
            // Task<int> task = Task.Run( () => {
            //     Thread.Sleep(1000);
            //     System.Console.WriteLine("Task is finished.");
            // });   
            // task.Wait(); // ❌  stop the main thread until the result is ready
            // // wait the task is completed, then move on to the next line

            // System.Console.WriteLine("After the task.");

            // sometimes we need to wait for the task to finish, but we don't want to block the main thread
            // we can use async and await
            // if we want to wait for multiple tasks, we can use Task.WhenAll or Task.WhenAny

            #endregion
            #region 4) Continuation - Task return value with `ContinueWith`
            // Task continuation = Task.Run(async () => await CalculateLen("Hello World"))
            //     .ContinueWith((taskWithResult) => // computed the result, we can use the result, and no waiting for its to finish.
            //     System.Console.WriteLine("Length is: " + taskWithResult.Result)); // ✅ this will not block the main thread

            // System.Console.WriteLine("Task is done!");


            //  string userInput;
            // do{
            //     System.Console.WriteLine("Enter a command:");
            //     userInput = Console.ReadLine();
            // }while(userInput != "exit");  // we can enter the commands 

            #endregion
            #region 5) Chain of Continuations
            // var tasks = new List<Task<int>>();
            // tasks.Add(Task.Run(() => CalculateLen("Hello")));
            // tasks.Add(Task.Run(() => CalculateLen("World")));
            // tasks.Add(Task.Run(() => CalculateLen("!!!")));

            // var continuation = Task.Factory.ContinueWhenAll(
            //     tasks.ToArray(), 
            //     (completedTasks) => {
            //         foreach (var task in completedTasks)
            //         {
            //             System.Console.WriteLine(task.Result); // Assuming CalculateLen returns an int
            //         }
            //     });


            // string userInput;
            // do{
            //     System.Console.WriteLine("Enter a command:");
            //     userInput = Console.ReadLine();
            // }while(userInput != "exit");  // we can enter the commands 

            #endregion
            #region 5) Cancel the task using cooperative cancellation with `CancellationToken`
            // `CancellationToken` is an object shared by the code that requests the cancellation'
            // and the code that performs the cancellation.

            // Cooperative cancellation happens when both the code that triggers the task 
            // and the code running within the task are aware of the cancellation, and they cooperate to perform it.

            var cts = new CancellationTokenSource(); // the class to provide such token
            var task = Task.Run(async () => await NeverEndingMethod(cts),
                cts.Token);  // ✅ the token is passed to the task

            string userInput;
            do{
                System.Console.WriteLine("Enter a command:");
                userInput = Console.ReadLine();
            }while(userInput != "cancel");

            cts.Cancel(); // cancel the task

            System.Console.WriteLine("Task is cancelled.");

            #endregion
        }

        static async Task NeverEndingMethod(CancellationTokenSource cts = default)
        {
            while(true)
            {
                cts.Token.ThrowIfCancellationRequested(); // ✅ check if the cancellation is requested
                // if(cts != default && cts.IsCancellationRequested)
                // {
                //     System.Console.WriteLine("Cancellation requested.");
                //     // return; // ❌ not good
                //     throw new OperationCanceledException(cts.Token); // ✅
                // }
                System.Console.WriteLine("Never ending method is running...");
                await Task.Delay(2000);
            }
        }

        async static Task<int> CalculateLen(string input){
            System.Console.WriteLine("CaluculateLen thread's ID: " + System.Threading.Thread.CurrentThread.ManagedThreadId);
            await Task.Delay(4000); // pause 4 seconds
            return input.Length;
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
        
    }
}