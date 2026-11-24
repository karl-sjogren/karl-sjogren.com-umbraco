using Shorthand.UmbracoHelpers;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Services.Navigation;
using UmbracoBlog.Umbraco.Features.Sitemaps;

namespace UmbracoBlog.Umbraco.Tests.Features.Sitemaps;

public class SitemapGeneratorTests {
    [Fact]
    public async Task GenerateSitemapXml_WhenCalledWithoutEntries_GeneratesEmptySitemapAsync() {
        // Arrange
        var publishedContentQuery = A.Dummy<IPublishedContentQuery>();
        var documentNavigationQueryService = A.Dummy<IDocumentNavigationQueryService>();
        var publishedContentStatusFilteringService = A.Dummy<IPublishedContentStatusFilteringService>();
        var friendlyPublishedContentHelper = A.Dummy<IFriendlyPublishedContentHelper>();

        var service = new SitemapGenerator(publishedContentQuery, documentNavigationQueryService, publishedContentStatusFilteringService, friendlyPublishedContentHelper, new FakeTimeProvider(), NullLogger<SitemapGenerator>.Instance);

        var stream = new MemoryStream();
        var entries = Array.Empty<SitemapEntry>();

        // Act
        await service.GenerateSitemapXmlAsync(entries, stream, TestCancellationToken);

        // Assert
        stream.Position = 0;
        using var streamReader = new StreamReader(stream);
        var sitemapXml = await streamReader.ReadToEndAsync(TestCancellationToken);
        sitemapXml.ShouldBe("""
<?xml version="1.0" encoding="utf-8"?>
<!--Generated at 2000-01-01T00:00:00Z.-->
<urlset xmlns:xhtml="http://www.w3.org/1999/xhtml" xmlns="http://www.sitemaps.org/schemas/sitemap/0.9" />
""", StringCompareShould.IgnoreLineEndings);
    }

    [Fact]
    public async Task GenerateSitemapXml_WhenCalledWithCompleteEntry_GeneratesExpectedSitemapAsync() {
        // Arrange
        var publishedContentQuery = A.Dummy<IPublishedContentQuery>();
        var documentNavigationQueryService = A.Dummy<IDocumentNavigationQueryService>();
        var publishedContentStatusFilteringService = A.Dummy<IPublishedContentStatusFilteringService>();
        var friendlyPublishedContentHelper = A.Dummy<IFriendlyPublishedContentHelper>();

        var service = new SitemapGenerator(publishedContentQuery, documentNavigationQueryService, publishedContentStatusFilteringService, friendlyPublishedContentHelper, new FakeTimeProvider(), NullLogger<SitemapGenerator>.Instance);

        var stream = new MemoryStream();
        var entries = new SitemapEntry[] {
            new("https://doofenshmirtz.inc/posts/platypus-problems", new DateTime(2026, 11, 24, 22, 38, 00))
        };

        // Act
        await service.GenerateSitemapXmlAsync(entries, stream, TestCancellationToken);

        // Assert
        stream.Position = 0;
        using var streamReader = new StreamReader(stream);
        var sitemapXml = await streamReader.ReadToEndAsync(TestCancellationToken);
        sitemapXml.ShouldBe("""
<?xml version="1.0" encoding="utf-8"?>
<!--Generated at 2000-01-01T00:00:00Z.-->
<urlset xmlns:xhtml="http://www.w3.org/1999/xhtml" xmlns="http://www.sitemaps.org/schemas/sitemap/0.9">
  <url>
    <loc>https://doofenshmirtz.inc/posts/platypus-problems</loc>
    <lastmod>2026-11-24T22:38:00+01:00</lastmod>
  </url>
</urlset>
""", StringCompareShould.IgnoreLineEndings);
    }
}
