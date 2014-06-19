// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileProcessingJob.cs" company="">
//   
// </copyright>
// <summary>
//   The file processing job.
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
    /// The file processing job.
    /// </summary>
    internal class FileProcessingJob
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

            CloudQueue fileQueue = queueClient.GetQueueReference(Constants.FileQueue);
            fileQueue.CreateIfNotExists();

            while (!this.isCancel)
            {
                CloudQueueMessage message;
                do
                {
                    message = fileQueue.GetMessage();
                    if (message != null)
                    {
                        var file = new FileInfo(message.AsString);
                        if (file.Exists)
                        {
                            // TODO: Process file...
                            file.Delete();
                            Console.WriteLine("File " + file.Name + " deleted.");
                        }
                        else
                        {
                            Console.WriteLine("ERROR: Received message for non-existing file");
                        }

                        fileQueue.DeleteMessage(message);
                    }
                }
                while (message != null);

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