using Azure.Storage.Blobs;
using Umbraco.Cms.Persistence.EFCore;
using Umbraco.StorageProviders.AzureBlob.IO;

namespace UmbracoBlog.Umbraco.Extensions;

public static class IUmbracoBuilderExtensions {
    public static IUmbracoBuilder ConfigureAspireServices(this IUmbracoBuilder builder, IHostApplicationBuilder hostApplicationBuilder) {
        if(Environment.GetEnvironmentVariable("RUNNING_ASPIRE") != bool.TrueString) {
            return builder;
        }

        // Register the Azure Blob File System with the Umbraco builder
        // and configure it to use the Aspire BlobStorageClient.
        builder
            .AddAzureBlobMediaFileSystem<BlobContainerClient>((options, blobContainerClient) => options.UseBlobContainerClient(blobContainerClient))
            .AddAzureBlobImageSharpCache();

        hostApplicationBuilder.AddAzureBlobContainerClient("umbraco-blog-media");

        if(hostApplicationBuilder.Configuration.GetConnectionString("umbraco-blog-redis") is not null) {
            hostApplicationBuilder.AddRedisDistributedCache(connectionName: "umbraco-blog-redis");
        }

        // Enrich the UmbracoDbContext to report telemetry to Aspire
        hostApplicationBuilder.EnrichSqlServerDbContext<UmbracoDbContext>();
        return builder;
    }
}
