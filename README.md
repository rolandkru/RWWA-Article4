Real World Windows Azure - Article 4
=============

This is a sample application I wrote for the "Real World Windows Azure"-Series in the Windows Developer Magazine.

Installation:
-------------
1. Create a windows azure subscription. See: http://www.windowsazure.com/de-de/pricing/free-trial/
2. Create a windows azure storage account. See: http://azure.microsoft.com/en-us/documentation/articles/storage-create-storage-account/
3. Update the class Constants.cs with the storage account name and key you just created.

4. Create a local directoy "C:\temp\rwwa" and generate some random files. You can change the path in the class Constants.cs.
5. Compile the RWWA_Article4 Project. Start the Application multiple times.

6. Create an Azure Scheduler Job, that generates every minute a message with the content "Make indexing" to a queue named "schedulerqueue" in the storage account created before. See http://azure.microsoft.com/en-us/documentation/services/scheduler/
7. Watch the console instances first indexing the files and then deleting the files in the directory in a distributed manner as soon as the first message from the scheduler is created.