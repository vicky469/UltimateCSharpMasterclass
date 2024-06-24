using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// ref: https://www.milanjovanovic.tech/blog/introduction-to-locking-and-concurrency-control-in-dotnet-6
namespace _16_AsynchronyAndMultithreading
{
    public class Counter
    {
        public int Value  {get; private set;}
        private object  _valueLock = new object();

        public void Increment()
        {
            lock(_valueLock){
                Value++;
            }
        }

        public void Decrement()
        {
            lock(_valueLock){
                Value--;
            }
        }
    }
    public class Lock
    {
        public static async void Execute(string[] args)
        {
            #region  1) ❌ Problem - the value is not thread-safe
            // solution is to add a simple lock in the Counter class
            // var counter = new Counter();
            // var tasks = new List<Task>();
            // for(int i = 0; i < 10; i++){
            //     tasks.Add(Task.Run(() => counter.Increment()));
            // }
            // for (int i = 0; i < 10; i++)
            // {
            //     tasks.Add(Task.Run(() => counter.Decrement()));
            // }
            // Task.WaitAll(tasks.ToArray());
            // System.Console.WriteLine("Counter value: " + counter.Value);
            
            // Result for running the same code:
            // 1st run Counter value: 0
            // 2nd run Counter value: -2

            #endregion

            #region  2) lock in async method
            var account = new BankAccount(0); 

            var tasks = new List<Task>
            {
                account.Deposit(100),
                account.Withdraw(100),
                account.Deposit(200)
            };

            await Task.WhenAll(tasks.ToArray());

            Console.WriteLine($"Final balance is: {account.GetBalance()}");

            #endregion

        }
    }

    public class BankAccount
    {
        private static readonly SemaphoreSlim _semaphore = new SemaphoreSlim(2,10);

        // Public property to get the balance. Note the private set.
        private decimal _balance;

        public BankAccount(decimal initialBalance = default)
        {
             _balance = initialBalance;
        }

        public async Task Deposit(decimal amount)
        {
            await _semaphore.WaitAsync(); // block the current thread until it can enter the semaphore

            _balance += amount;
            Console.WriteLine($"Deposited {amount}, new balance is: {_balance}");
            _semaphore.Release(); // release the semaphore
        }

        public async Task Withdraw(decimal amount)
        {
            await _semaphore.WaitAsync(); // Wait to enter the semaphore
            try
            {
                if (_balance >= amount)
                {
                    _balance -= amount;
                    Console.WriteLine($"Withdrew {amount}, new balance is: {_balance}");
                }
                else
                {
                    Console.WriteLine("Withdrawal attempt failed due to insufficient funds.");
                }
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public decimal GetBalance() => _balance;
    }
}