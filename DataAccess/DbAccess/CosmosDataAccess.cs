using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using Dapper;
using System.Data;
using ServiceStack.Text;
using Microsoft.Azure.Cosmos;
using System.Net;
using System.Drawing;
using DataAccess.Models;

namespace DataAccess.DbAccess
{
    public class CosmosDataAccess : ICosmosDataAccess
    {
        private readonly IConfiguration _config;
        private CosmosClient cosmosClient;
        private Database database;
        private Container container;
        private string databaseId = "ToDoList";
        private string containerId = "Items";

        public CosmosDataAccess(IConfiguration config)
        {
            _config = config;
            cosmosClient = new CosmosClient(_config.GetConnectionString("Cosmos"));
            
        }


        private async Task CreateDatabaseAsync()
        {
            // Create a new database
            database = await cosmosClient.CreateDatabaseIfNotExistsAsync(databaseId);
            Console.WriteLine("Created Database: {0}\n", database.Id);
        }

        private async Task CreateContainerAsync()
        {
            await CreateDatabaseAsync();
            // Create a new container
            container = await database.CreateContainerIfNotExistsAsync(containerId, "/id");
            Console.WriteLine("Created Container: {0}\n", container.Id);
        }

        private async Task ScaleContainerAsync()
        {
            await CreateContainerAsync();
            // Read the current throughput
            try
            {
                int? throughput = await container.ReadThroughputAsync();
                if (throughput.HasValue)
                {
                    Console.WriteLine("Current provisioned throughput : {0}\n", throughput.Value);
                    int newThroughput = throughput.Value + 100;
                    // Update throughput
                    await container.ReplaceThroughputAsync(newThroughput);
                    Console.WriteLine("New provisioned throughput : {0}\n", newThroughput);
                }
            }
            catch (CosmosException cosmosException) when (cosmosException.StatusCode == HttpStatusCode.BadRequest)
            {
                Console.WriteLine("Cannot read container throuthput.");
                Console.WriteLine(cosmosException.ResponseBody);
            }

        }
        public async Task AddItemToContainerAsync(UserModel user)
        {
            await CreateContainerAsync();
            try
            {
                // Read the item to see if it exists.  
                ItemResponse<UserModel> userResponse = await container.ReadItemAsync<UserModel>(user.Id, new PartitionKey(user.Id));
                Console.WriteLine("Item in database with id: {0} already exists\n", userResponse.Resource.Id);
            }
            catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                // Create an item in the container representing the Andersen family. Note we provide the value of the partition key for this item, which is "Andersen"
                ItemResponse<UserModel> userResponse = await container.CreateItemAsync<UserModel>(user, new PartitionKey(user.Id));

                // Note that after creating the item, we can access the body of the item with the Resource property off the ItemResponse. We can also access the RequestCharge property to see the amount of RUs consumed on this request.
                Console.WriteLine("Created item in database with id: {0} Operation consumed {1} RUs.\n", userResponse.Resource.Id, userResponse.RequestCharge);
            }
        }
        public async Task<List<UserModel>> QueryItemsAsync(string sqlQueryText)
        {
            await CreateContainerAsync();
            Console.WriteLine("Running query: {0}\n", sqlQueryText);

            QueryDefinition queryDefinition = new QueryDefinition(sqlQueryText);
            FeedIterator<UserModel> queryResultSetIterator = container.GetItemQueryIterator<UserModel>(queryDefinition);

            List<UserModel> users = new List<UserModel>();

            while (queryResultSetIterator.HasMoreResults)
            {
                FeedResponse<UserModel> currentResultSet = await queryResultSetIterator.ReadNextAsync();
                foreach (UserModel user in currentResultSet)
                {
                    users.Add(user);
                }
            }
            return users;
        }

        public async Task ReplaceFamilyItemAsync(UserModel user)
        {
            await CreateContainerAsync();
            //ItemResponse<UserModel> userResponse = await container.ReadItemAsync<UserModel>(user.Id, new PartitionKey(user.Id));
            //var itemBody = userResponse.Resource;

            //// update registration status from false to true
            //itemBody.IsRegistered = true;
            //// update grade of child
            //itemBody.Children[0].Grade = 6;

            // replace the item with the updated content
            ItemResponse<UserModel> userResponse = await container.ReplaceItemAsync<UserModel>(user, user.Id, new PartitionKey(user.Id));
            //Console.WriteLine("Updated Family [{0},{1}].\n \tBody is now: {2}\n", itemBody.LastName, itemBody.Id, wakefieldFamilyResponse.Resource);
        }
        public async Task DeleteFamilyItemAsync(string partitionKeyValue, string userId)
        {
            await CreateContainerAsync();
            //var partitionKeyValue = "Wakefield";
            //var familyId = "Wakefield.7";

            // Delete an item. Note we must provide the partition key value and id of the item to delete
            ItemResponse<UserModel> wakefieldFamilyResponse = await container.DeleteItemAsync<UserModel>(userId, new PartitionKey(partitionKeyValue));
            //Console.WriteLine("Deleted Family [{0},{1}]\n", partitionKeyValue, familyId);
        }


        //public async Task<IEnumerable<T>> LoadData<T, U>(
        //    string storedProcedures,
        //    U parameters,
        //    string connectionId = "Default")
        //{
        //    using IDbConnection connection = new SqlConnection(_config.GetConnectionString(connectionId));
        //    return await connection.QueryAsync<T>(storedProcedures, parameters,
        //        commandType: CommandType.StoredProcedure);
        //}

        //public async Task SaveData<T>(
        //    string storedProcedures,
        //    T parameters,
        //    string connectionId = "Default")
        //{
        //    using IDbConnection connection = new SqlConnection(_config.GetConnectionString(connectionId));
        //    await connection.ExecuteAsync(storedProcedures, parameters,
        //        commandType: CommandType.StoredProcedure);
        //}

    }
}
