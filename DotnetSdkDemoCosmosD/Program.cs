using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using System.Net;

namespace DotnetSdkDemoCosmosD
{
    public class Program
    {
        private static async Task Main(string[] args)
        {
            await QueryForDocuments();
        }

        private static async Task QueryForDocuments()
        {
            IConfiguration config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            var endpoint = config.GetSection("CosmosEndpoint").Value;
            var masterKey = config.GetSection("CosmosMasterKey").Value;

            using var client = new CosmosClient(endpoint, masterKey);
            var container = client.GetContainer("Families", "Families");
            var sql = "SELECT * FROM c WHERE ARRAY_LENGTH(c.children) > 1";
            var iterator = container.GetItemQueryIterator<dynamic>(sql);
            var page = await iterator.ReadNextAsync();

            foreach (var doc in page)
            {
                Console.WriteLine($"Family {doc.id} has {doc.children.Count} children");
            }
        }
    }
}