namespace Adapter.Azure
{
    using Domain.Bases;
    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Blob;
    using Newtonsoft.Json;
    using Process.Ports;
    using System.Threading.Tasks;

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

            string key = $"{typeof(TDocument).FullName}-{document.Id}";

            CloudBlockBlob blob = container.GetBlockBlobReference(key);
            blob.Properties.ContentType = "application/json";

            string json = JsonConvert.SerializeObject(document);

            await blob.UploadTextAsync(json);
        }

        public async Task<TDocument> GetAsync<TDocument>(string id)
            where TDocument : AggregateState
        {
            string key = $"{typeof(TDocument).FullName}-{id}";

            CloudBlobContainer container = client
                .GetContainerReference(containerName);

            CloudBlockBlob blob = container.GetBlockBlobReference(key);

            string json = await blob.DownloadTextAsync();

            return JsonConvert.DeserializeObject<TDocument>(json);
        }

        public async Task DeleteAsync<TDocument>(string id)
            where TDocument : AggregateState
        {
            string key = $"{typeof(TDocument).FullName}-{id}";

            CloudBlobContainer container = client
                .GetContainerReference(containerName);

            CloudBlockBlob blob = container.GetBlockBlobReference(key);

            await blob.DeleteIfExistsAsync();
        }

        public async Task<bool> ExistsAsync<TDocument>(string id)
            where TDocument : AggregateState
        {
            string key = $"{typeof(TDocument).FullName}-{id}";

            CloudBlobContainer container = client
                .GetContainerReference(containerName);

            CloudBlockBlob blob = container.GetBlockBlobReference(key);

            return await blob.ExistsAsync();
        }
    }
}
