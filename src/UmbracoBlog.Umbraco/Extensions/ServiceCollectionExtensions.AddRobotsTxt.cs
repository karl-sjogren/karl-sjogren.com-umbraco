using RobotsTxt;

namespace UmbracoBlog.Umbraco.Extensions;

public static partial class ServiceCollectionExtensions {
    public static WebApplicationBuilder AddRobotsTxt(this WebApplicationBuilder builder) {
        ArgumentNullException.ThrowIfNull(builder);

        var umbracoApplicationUrl = builder.Configuration["Umbraco:CMS:WebRouting:UmbracoApplicationUrl"];
        builder.Services.AddStaticRobotsTxt(x => BuildRobotsTxt(x, umbracoApplicationUrl));

        return builder;
    }

    internal static RobotsTxtOptionsBuilder BuildRobotsTxt(RobotsTxtOptionsBuilder robotBuilder, string? umbracoApplicationUrl) {
        return robotBuilder
            .AddSitemap($"{umbracoApplicationUrl}sitemap.xml")
            .AddSection(section =>
                section
                    .AddUserAgent("*")
                    .Disallow("/app_data/")
                    .Disallow("/app_plugins/")
                    .Disallow("/umbraco/")
                    .Disallow("/install")
            );
    }
}
