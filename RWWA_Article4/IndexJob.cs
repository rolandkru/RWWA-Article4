// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IndexJob.cs" company="">
//   
// </copyright>
// <summary>
//   The index job.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace RWWA_Article4
{
    using System;
    using System.IO;
    using System.Threading;

    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Auth;
    using Microsoft.WindowsAzure.Storage.Queue;

    /// <summary>
    /// The index job.
    /// </summary>
    internal class IndexJob
    {
        private bool isCancel = false;

        /// <summary>
        /// The work.
        /// </summary>
        public void Work()
        {
            var credentials = new StorageCredentials(Constants.StorageAccountName, Constants.StorageAccountKey);

            var storageAccount = new CloudStorageAccount(credentials, true);
            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();
            
            CloudQueue schedulerQueue = queueClient.GetQueueReference(Constants.SchedulerQueue);
            schedulerQueue.CreateIfNotExists();

            var fileQueue = queueClient.GetQueueReference(Constants.FileQueue);
            fileQueue.CreateIfNotExists();

            while (!this.isCancel)
            {
                CloudQueueMessage schedulerMessage;
                do
                {
                    schedulerMessage = schedulerQueue.GetMessage();
                    if (schedulerMessage != null)
                    {
                        Console.WriteLine("Received indexing message. Start Indexing...");

                        var directory = new DirectoryInfo(Constants.LocalDirectory);
                        foreach (var file in directory.EnumerateFiles())
                        {
                            var fileMessage = new CloudQueueMessage(file.FullName);
                            fileQueue.AddMessage(fileMessage);
                        }

                        Console.WriteLine("File queue filled.");

                        schedulerQueue.DeleteMessage(schedulerMessage);
                    }
                }
                while (schedulerMessage != null);

                Thread.Sleep(TimeSpan.FromSeconds(5));
            }
        }

        /// <summary>
        /// The cancel.
        /// </summary>
        public void Cancel()
        {
            this.isCancel = true;
        }
    }
}