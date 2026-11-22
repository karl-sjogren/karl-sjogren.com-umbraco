using Umbraco.Aspire.AppHost.Extensions;

var builder = DistributedApplication.CreateBuilder(args);

var umbraco = builder.AddUmbracoProject("umbraco-blog");

builder
    .AddSqlServer(umbraco, "umbraco-blog-sql", "umbraco-blog-sqldb")
    .AddAzureStorage(umbraco, "umbraco-blog-storage", "umbraco-blog-media")
    .AddRedisCache(umbraco, "umbraco-blog-redis")
    .AddFrontendProject();

builder.Build().Run();
