# Azure Cosmos DB - Delete/Backup documents by Query

This is a very simple Visual Studio app coded in C#/dotnet core using the CosmosDB SDK to something that is not possible to do otherwise in a simple way: delete a set of documents from CosmosDB based on a SELECT query.

To use the code, clone the repo and edit the code to include your connection string, database and container name, as well as the SELECT query. Keep it a `SELECT *`, especially if you want to use the backup option.

There are two flags: one (`backupDocuments`) controls whether or not to do backup of the json files to a pre-defined folder (`backupFolder`, the other to say whether or not you want to delete the files (eventually after the backup) - `deleteDocuments`. It's **highly recommended** you test the backup mode before turning on deletion. The settings to review are:

```
private const string cosmosDBConnectionString = "AccountEndpoint=https://....";
private const string cosmosDBDatabaseName = "name_of_database";
private const string cosmosDBContainerName = "name_of_container";
private const string cosmosDbSelectQuery = "SELECT * FROM c WHERE c.pk = '2001'";
private const bool backupDocuments = true;
private const string backupFolder = "backups";
private const bool deleteDocuments = true;
```

Because I'm using the `dynamic` "type" when reading from CosmosDB, and I need to access the item's Id and Partition Key to do the deletion. you may need to replace the name of the partition key in the code for it to run correctly. E.g., replace references to `item.pk` to `item.YOUR_PK_NAME`. Note that not fixing this doesn't cause a compilation error becasue of the use of dynamic! It only fails in runtime.

This doesn't use the bulk execution, and reads > backups > deletes one by one, so don't expect it to be fast.

Hope this helps.
