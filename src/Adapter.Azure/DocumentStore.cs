namespace Adapter.Azure
{
    using System.Threading.Tasks;
    using Domain.Bases;
    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Blob;
    using Newtonsoft.Json;
    using Process.Ports;

    public class DocumentStore : IStoreDocuments
    {
        readonly string containerName;
        readonly CloudStorageAccount cloudStorageAccount;
        readonly CloudBlobClient client;

        public DocumentStore(string connectionString, string containerName)
        {
            this.containerName = containerName;

            cloudStorageAccount = CloudStorageAccount.Parse(connectionString);
            client = cloudStorageAccount.CreateCloudBlobClient();
        }

        public async Task StoreAsync<TDocument>(TDocument document)
            where TDocument : AggregateState
        {
            CloudBlobContainer container = client
                .GetContainerReference(containerName);

            await container.CreateIfNotExistsAsync();

            string key = $"{typeof(TDocument).Name}:{document.Id}";

            CloudBlockBlob blob = container.GetBlockBlobReference(key);

            string json = JsonConvert.SerializeObject(document);

            await blob.UploadTextAsync(json);
        }

        public Task<TDocument> GetAsync<TDocument>(string id)
            where TDocument : AggregateState
        {
            throw new System.NotImplementedException();
        }
    }
}
