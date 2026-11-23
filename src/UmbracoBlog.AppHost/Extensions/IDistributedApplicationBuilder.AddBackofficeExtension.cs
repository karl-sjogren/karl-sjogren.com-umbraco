
using Aspire.Hosting.Dotnet;

namespace UmbracoBlog.AppHost.Extensions;

internal static partial class IDistributedApplicationBuilderExtensions {
    public static IDistributedApplicationBuilder AddBackofficeExtension(
            this IDistributedApplicationBuilder builder,
            IResourceBuilder<DotnetProjectResource> umbracoProject) {
        if(!builder.ExecutionContext.IsRunMode) {
            return builder;
        }

        var backofficeExtension = builder
            .AddJavaScriptApp("umbraco-blog-backoffice-extension", "../UmbracoBlog.BackofficeExtension/Client", runScriptName: "watch")
            .WithYarn();

        umbracoProject
            .WaitFor(backofficeExtension);

        return builder;
    }
}
