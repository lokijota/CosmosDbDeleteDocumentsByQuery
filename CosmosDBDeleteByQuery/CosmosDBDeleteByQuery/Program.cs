using System;
using Microsoft.Azure.Cosmos;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CosmosDBDeleteByQuery
{
    class Program
    {
        // VARIABLES TO UPDATE
        private const string cosmosDBConnectionString = "AccountEndpoint=https://....";
        private const string cosmosDBDatabaseName = "name_of_database";
        private const string cosmosDBContainerName = "name_of_container";
        private const string cosmosDbSelectQuery = "SELECT * FROM c WHERE c.pk = '2001'";
        private const bool backupDocuments = true;
        private const string backupFolder = "backups";
        private const bool deleteDocuments = true;

        // BECAUSE THE CODE BELOW USES dynamic, YOU'LL NEED TO CHANGE item.pk to item.YOUR_OK_PROPERTY
        // NOTE THAT THIS IS CAUSES A RUNTIME ERROR, NOT COMPILATION ERROR

        static async Task Main(string[] args)
        {
            Console.WriteLine("**** CosmosDB - Backup and delete documents by query ****");
            Console.WriteLine("Documents to backup/delete: " + cosmosDbSelectQuery);
            Console.WriteLine("Backup documents? " + backupDocuments);
            Console.WriteLine("Delete documents? " + deleteDocuments);
            Console.WriteLine();

            if(backupDocuments)
            {
                Directory.CreateDirectory(backupFolder);
            }

            // set up connection to CosmosDB
            CosmosClient cosmosClient = new CosmosClient(cosmosDBConnectionString, new CosmosClientOptions() { AllowBulkExecution = true });
            Database database = cosmosClient.GetDatabase(cosmosDBDatabaseName);
            Container container = database.GetContainer(cosmosDBContainerName);

            Console.WriteLine("***Reading documents (.), backing up (v) and deleting (x) according to configuration ***");

            List<IdPkPair> ids = new List<IdPkPair>();

            using (FeedIterator<dynamic> feedIterator = container.GetItemQueryIterator<dynamic>(cosmosDbSelectQuery))
            {
                while (feedIterator.HasMoreResults)
                {
                    FeedResponse<dynamic> response = await feedIterator.ReadNextAsync();
                    foreach (var item in response)
                    {
                        Console.Write('.');
                        ids.Add(new IdPkPair() {  DocumentId =item.id, PartitionKey = item.pk });

                        if(backupDocuments)
                        {
                            File.WriteAllText(string.Format("{0}/{1}-{2}.json", backupFolder, item.id, item.pk), item.ToString());
                            Console.Write('v');
                        }

                        if (deleteDocuments)
                        {
                            await container.DeleteItemAsync<dynamic>(item.id.ToString(), new PartitionKey(item.pk.ToString()));
                            Console.Write('x');
                        }

                    }
                }
            }

            Console.WriteLine("Records processed: {0}", ids.Count());
            Console.ReadLine();
        }
    }
}
