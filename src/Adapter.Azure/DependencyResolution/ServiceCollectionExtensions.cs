namespace Adapter.Azure.DependencyResolution
{
    using Domain.Utilities;
    using Microsoft.Extensions.DependencyInjection;
    using Process.Ports;

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection UseAzureDocumentStorage(
            this IServiceCollection services,
            string connectionString,
            string containerName)
        {
            Ensure.IsNotNullOrEmpty(connectionString, nameof(connectionString));
            Ensure.IsNotNullOrEmpty(containerName, nameof(containerName));

            services.AddTransient<IStoreDocuments>(provider =>
                new DocumentStore(connectionString, containerName));

            return services;
        }
    }
}
