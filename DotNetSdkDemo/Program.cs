using System;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;

namespace DotNetSdkDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            QueryForDocuments().Wait();
        }

        private async static Task QueryForDocuments()
        {
            var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            var endpoint = config["CosmosEndpoint"];
            var masterKey = config["CosmosMasterKey"];

            using (var client = new CosmosClient(accountEndpoint, masterKey))
            {
                var container = client.GetContainer("Families", "Families");
                var sql = "SELECT * FROM c WHERE ARRAY_LENGTH(c.children) > 1";
                var iterator = container.GetItemQueryIterator<dynamic>(sql);
                var page = await iterator.ReadNextAsync();

                foreach(var doc in page)
                {
                    Console.WriteLine($"Family {doc} as {doc.children.Count} children");
                }
                Console.ReadLine();
            }
        }
    }
}
