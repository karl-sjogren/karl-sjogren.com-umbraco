using System.Text;
using System.Xml;
using Shorthand.UmbracoHelpers;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Services.Navigation;
using UmbracoBlog.Core.Documents.Composites;

namespace UmbracoBlog.Umbraco.Features.Sitemaps;

public class SitemapGenerator : ISitemapGenerator {
    private readonly IPublishedContentQuery _publishedContentQuery;
    private readonly IDocumentNavigationQueryService _documentNavigationQueryService;
    private readonly IPublishedContentStatusFilteringService _publishedContentStatusFilteringService;
    private readonly IFriendlyPublishedContentHelper _friendlyPublishedContentHelper;
    private readonly TimeProvider _timeProvider;
    private readonly ILogger<SitemapGenerator> _logger;

    private readonly string[] _allowedModelAliases = [
        Core.Documents.StartPage.ModelTypeAlias,
        Core.Documents.ContentPage.ModelTypeAlias,
        Core.Documents.BlogPost.ModelTypeAlias
    ];

    public SitemapGenerator(
            IPublishedContentQuery publishedContentQuery,
            IDocumentNavigationQueryService documentNavigationQueryService,
            IPublishedContentStatusFilteringService publishedContentStatusFilteringService,
            IFriendlyPublishedContentHelper friendlyPublishedContentHelper,
            TimeProvider timeProvider,
            ILogger<SitemapGenerator> logger) {
        _publishedContentQuery = publishedContentQuery;
        _documentNavigationQueryService = documentNavigationQueryService;
        _publishedContentStatusFilteringService = publishedContentStatusFilteringService;
        _friendlyPublishedContentHelper = friendlyPublishedContentHelper;
        _timeProvider = timeProvider;
        _logger = logger;
    }

    public Task GenerateSitemapAsync(Stream stream, CancellationToken cancellationToken) {
        var entries = new List<SitemapEntry>();

        var pages = _publishedContentQuery
            .ContentAtRoot()
            .SelectMany(root => root.DescendantsOrSelf(
                _documentNavigationQueryService,
                _publishedContentStatusFilteringService))
            .Where(page => _allowedModelAliases.Contains(page.ContentType.Alias) && page.IsPublished())
            .Where(page => {
                if(page is ISeoComposite seoPage) {
                    return !seoPage.BlockSearchEngineIndexing;
                }

                return true;
            })
            .Select(page => new SitemapEntry(
                _friendlyPublishedContentHelper.Url(page, mode: UrlMode.Absolute),
                page.UpdateDate
            ));

        return GenerateSitemapXmlAsync(pages, stream, cancellationToken);
    }

    internal async Task GenerateSitemapXmlAsync(IEnumerable<SitemapEntry> entries, Stream stream, CancellationToken cancellationToken) {
        var settings = new XmlWriterSettings {
            Indent = true,
            CloseOutput = false,
            Encoding = Encoding.UTF8,
            OmitXmlDeclaration = false,
            NewLineHandling = NewLineHandling.Entitize,
            ConformanceLevel = ConformanceLevel.Document,
            Async = true
        };

        await using var writer = XmlWriter.Create(stream, settings);

        await writer.WriteStartDocumentAsync();

        await writer.WriteCommentAsync($"Generated at {_timeProvider.GetUtcNow():yyyy-MM-ddTHH:mm:ssZ}.");

        await writer.WriteStartElementAsync(null, "urlset", "http://www.sitemaps.org/schemas/sitemap/0.9");
        await writer.WriteAttributeStringAsync("xmlns", "xhtml", null, "http://www.w3.org/1999/xhtml");

        foreach(var entry in entries.Take(50_000)) {
            if(cancellationToken.IsCancellationRequested) {
                break;
            }

            await writer.WriteStartElementAsync(null, "url", null);

            await writer.WriteElementStringAsync(null, "loc", null, entry.Url);
            await writer.WriteElementStringAsync(null, "lastmod", null, entry.LastModified.ToString("yyyy-MM-ddTHH:mm:sszzz"));

            await writer.WriteEndElementAsync();
        }

        await writer.WriteEndElementAsync();
        await writer.WriteEndDocumentAsync();
    }
}
