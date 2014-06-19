// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="">
//   
// </copyright>
// <summary>
//   The program.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace RWWA_Article4
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// The program.
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// The main.
        /// </summary>
        /// <param name="args">
        /// The args.
        /// </param>
        private static void Main(string[] args)
        {
            Console.WriteLine("Waiting 5 seconds for demonstration.");
            Thread.Sleep(5000);

            var indexJob = new IndexJob();
            var fileProcessingJob = new FileProcessingJob();

            var indexTask = new Task(indexJob.Work);
            indexTask.Start();

            var fileProcessingTask = new Task(fileProcessingJob.Work);
            fileProcessingTask.Start();

            Console.WriteLine("Up and listening to the queues. Press enter to exit.");
            Console.ReadLine();

            Console.WriteLine("Waiting for tasks to finish...");
            indexTask.Wait(10000);
            fileProcessingTask.Wait(10000);
        }
    }
}