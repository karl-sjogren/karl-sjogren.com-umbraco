using Aspire.Hosting.Dotnet;

namespace Umbraco.Aspire.AppHost.Extensions;

internal static partial class IDistributedApplicationBuilderExtensions {
    public static IResourceBuilder<DotnetProjectResource> AddUmbracoProject(this IDistributedApplicationBuilder builder, string projectName) {
        var project = builder.AddDotnetProject(projectName, "../UmbracoBlog.Umbraco/UmbracoBlog.Umbraco.csproj")
            .WithExternalHttpEndpoints()
            .WithEnvironment("Umbraco__CMS__Unattended__InstallUnattended", bool.TrueString)
            .WithEnvironment("Umbraco__CMS__Unattended__UnattendedUserName", "blog")
            .WithEnvironment("Umbraco__CMS__Unattended__UnattendedUserEmail", "blog@example.com")
            .WithEnvironment("Umbraco__CMS__Unattended__UnattendedUserPassword", "password123!");

        if(builder.ExecutionContext.IsRunMode) {
            project
                .WithUrlForEndpoint("http", u => u.DisplayLocation = UrlDisplayLocation.DetailsOnly)
                .WithUrlForEndpoint("https", _ => new() { Url = "/umbraco", DisplayText = "Umbraco Backoffice" });
        }

        return project;
    }
}
