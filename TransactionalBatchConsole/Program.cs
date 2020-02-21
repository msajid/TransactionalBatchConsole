using Microsoft.Azure.Cosmos;
using System;
using System.Threading.Tasks;
using TransactionalBatchConsole.Models;

namespace TransactionalBatchConsole
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var databaseName = "MoviesDb";
            var containerName = "MoviesContainer";

            using (var cosmosClient =
                new CosmosClient("https://localhost:8081",
                                 "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw=="))
            {
                var cosmosDatabase = await cosmosClient
                                            .CreateDatabaseIfNotExistsAsync(databaseName);

                var cosmosContainer = await cosmosClient
                                            .GetDatabase(databaseName)
                                            .CreateContainerIfNotExistsAsync(containerName, "/PartitionKey");

                var container = cosmosContainer.Container;

                var ticket1 = new ShowBooking() { id = "13", PartitionKey = "12345-frozen2" };
                var ticket2 = new ShowBooking() { id = "14", PartitionKey = "12345-frozen2" };
                var ticket3 = new ShowBooking() { id = "15", PartitionKey = "12345-frozen2" };

                var batch = container
                            .CreateTransactionalBatch(new PartitionKey("12345-frozen2"))
                            .CreateItem(ticket1)
                            .CreateItem(ticket2)
                            .CreateItem(ticket3);

                var transactionResult = await batch.ExecuteAsync();

                if (transactionResult.IsSuccessStatusCode)
                {
                    //success logic goes here 
                    var ticket1result = transactionResult.GetOperationResultAtIndex<ShowBooking>(0);
                    var ticket2result = transactionResult.GetOperationResultAtIndex<ShowBooking>(1);
                    var ticket3result = transactionResult.GetOperationResultAtIndex<ShowBooking>(2);

                }
            }
        }
    }
}
