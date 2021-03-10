# Azure Cosmos DB - Delete/Backup documents by Query

This is a very simple Visual Studio app coded in C#/dotnet core using the CosmosDB SDK to something that is not possible to do otherwise in a simple way: delete a set of documents from CosmosDB based on a SELECT query.

To use the code, clone the repo and edit the code to include your connection string, database and container name, as well as the SELECT query. Keep it a `SELECT *`, especially if you want to use the backup option.

There are two flags: one controls whether or not to do backup of the json files to a pre-defined folder, the other to say whether or not you want to delete the files (eventually after the backup). It's highly recommended you test 
