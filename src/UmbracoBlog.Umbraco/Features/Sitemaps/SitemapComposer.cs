using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Web.Common.ApplicationBuilder;
using Umbraco.Cms.Web.Common.Routing;

namespace UmbracoBlog.Umbraco.Features.Sitemaps;

public class SitemapComposer : IComposer {
    public void Compose(IUmbracoBuilder builder) {
        builder.Services.AddScoped<ISitemapGenerator, SitemapGenerator>();
        builder.Services.AddTransient<SitemapMiddleware>();

        // Configure the request options to handle sitemap requests as server-side requests
        builder.Services.Configure<UmbracoRequestOptions>(options => {
            var handleAsServerSideRequest = options.HandleAsServerSideRequest;
            options.HandleAsServerSideRequest = request =>
                SitemapMiddleware.IsSitemapRequest(request) || handleAsServerSideRequest(request);
        });

        builder.Services.Configure<UmbracoPipelineOptions>(options => {
            options.AddFilter(new UmbracoPipelineFilter(
                "SitemapMiddleware",
                postRouting: app => app.UseMiddleware<SitemapMiddleware>()
            ));
        });
    }
}
