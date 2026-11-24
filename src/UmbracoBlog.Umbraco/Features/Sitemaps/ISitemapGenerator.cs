namespace UmbracoBlog.Umbraco.Features.Sitemaps;

public interface ISitemapGenerator {
    Task GenerateSitemapAsync(Stream stream, CancellationToken cancellationToken);
}
