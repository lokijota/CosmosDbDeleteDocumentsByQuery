# Azure Cosmos DB - Delete/Backup documents by Query

This is a very simple Visual Studio app coded in C#/dotnet core using the CosmosDB SDK to something that is not possible to do otherwise in a simple way: delete a set of documents from CosmosDB based on a SELECT query.

To use the code, clone the repo and edit the code to include your connection string, database and container name, as well as the SELECT query. Keep it a `SELECT *`, especially if you want to use the backup option.

There are two control flags: one (`backupDocuments`) controls whether or not to do backup of the json files to a pre-defined folder (`backupFolder`), the other is used to control whether or not you want to delete the records from CosmosDB (eventually after the backup) - `deleteDocuments`. It's **highly recommended** you test the backup mode before turning on deletion. The settings to review are:

```
private const string cosmosDBConnectionString = "AccountEndpoint=https://....";
private const string cosmosDBDatabaseName = "name_of_database";
private const string cosmosDBContainerName = "name_of_container";
private const string cosmosDbSelectQuery = "SELECT * FROM c WHERE c.pk = '2001'";
private const bool backupDocuments = true;
private const string backupFolder = "backups";
private const bool deleteDocuments = true;
```

Because I'm using the `dynamic` "type" when reading from CosmosDB, and I need to access the item's Partition Key to do the deletion, you will probably need to replace the name of the partition key in the code for it to run correctly. E.g., replace references to `item.pk` to `item.YOUR_PK_NAME`. Note that not fixing this doesn't cause a compilation error because of the use of `dynamic`! It only fails in runtime.

This doesn't use the bulk execution, and reads > backups > deletes one by one, so don't expect it to be fast.

## Why this?

I've had in several occasions needed to delete records en-masse from Cosmos DB. I always get to the same findings:

- It's not possible in the UI (there's no Delete statement)
- If you use a Stored Procedure in Javascript, it doesn't work cross-partition
- Using TTL expiration only applies to *all* the records in a container, it's not selective, and it's asynchronous -- sometimes you change back the TTL one hour later and all your records will be back.

So yeah.
