namespace Umbraco.Aspire.AppHost.Extensions;

internal static partial class IDistributedApplicationBuilderExtensions {
    public static IDistributedApplicationBuilder AddFrontendProject(
            this IDistributedApplicationBuilder builder) {
        if(!builder.ExecutionContext.IsRunMode) {
            return builder;
        }

        builder
            .AddViteApp("umbraco-blog-frontend-project", "../UmbracoBlog.Frontend/", runScriptName: "dev")
            .WithYarn()
            .WithEndpoint("http", e => e.Port = 5010);

        return builder;
    }
}
