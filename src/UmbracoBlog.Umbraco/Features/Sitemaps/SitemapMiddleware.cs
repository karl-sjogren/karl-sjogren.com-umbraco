namespace UmbracoBlog.Umbraco.Features.Sitemaps;

public class SitemapMiddleware : IMiddleware {
    private readonly ISitemapGenerator _sitemapGenerator;

    public SitemapMiddleware(ISitemapGenerator sitemapGenerator) {
        _sitemapGenerator = sitemapGenerator;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next) {
        if(!IsSitemapRequest(context.Request)) {
            await next(context);
            return;
        }

        context.Response.ContentType = "application/xml; charset=utf-8";
        await _sitemapGenerator.GenerateSitemapAsync(context.Response.Body, context.RequestAborted);
    }

    internal static bool IsSitemapRequest(HttpRequest request) =>
        request.Path.StartsWithSegments("/sitemap.xml", StringComparison.OrdinalIgnoreCase);
}
