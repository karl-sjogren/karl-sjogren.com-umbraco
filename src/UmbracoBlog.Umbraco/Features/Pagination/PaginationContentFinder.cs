using System.Text.RegularExpressions;
using Microsoft.Extensions.Options;
using Umbraco.Cms.Core.Configuration.Models;
using Umbraco.Cms.Core.PublishedCache;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Web;

namespace UmbracoBlog.Umbraco.Features.Pagination;

public partial class PaginationContentFinder : IContentFinder {
    private readonly IUmbracoContextAccessor _umbracoContextAccessor;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IDocumentUrlService _documentUrlService;
    private readonly IPublishedContentCache _publishedContentCache;
    private readonly IOptionsMonitor<WebRoutingSettings> _routingSettings;
    private readonly ILogger<PaginationContentFinder> _logger;

    public PaginationContentFinder(
            IUmbracoContextAccessor umbracoContextAccessor,
            IHttpContextAccessor httpContextAccessor,
            IDocumentUrlService documentUrlService,
            IPublishedContentCache publishedContentCache,
            IOptionsMonitor<WebRoutingSettings> routingSettings,
            ILogger<PaginationContentFinder> logger) {
        _umbracoContextAccessor = umbracoContextAccessor;
        _httpContextAccessor = httpContextAccessor;
        _documentUrlService = documentUrlService;
        _publishedContentCache = publishedContentCache;
        _routingSettings = routingSettings;
        _logger = logger;
    }

    public Task<bool> TryFindContent(IPublishedRequestBuilder request) {
        var httpContext = _httpContextAccessor.HttpContext;
        if(httpContext == null || !_umbracoContextAccessor.TryGetUmbracoContext(out var umbracoContext)
                || (request.Domain == null && _routingSettings.CurrentValue.UseStrictDomainMatching)) {
            return Task.FromResult(false);
        }

        var path = request.Domain == null
            ? request.AbsolutePathDecoded
            : DomainUtilities.PathRelativeToDomain(request.Domain.Uri, request.AbsolutePathDecoded);

        path = path.TrimEnd('/');
        var lastSlash = path.LastIndexOf('/', StringComparison.Ordinal);
        if(lastSlash < 0 || path.EndsWith('/', StringComparison.Ordinal)) {
            return Task.FromResult(false);
        }

        var pagePath = path[(lastSlash + 1)..];
        var page = ParsePagePath(pagePath);
        if(page is null) {
            return Task.FromResult(false);
        }

        var parentPath = lastSlash == 0 ? "/" : path[..lastSlash];
        var key = _documentUrlService.GetDocumentKeyByRoute(parentPath, request.Culture, request.Domain?.ContentId, umbracoContext.InPreviewMode);
        if(key is null) {
            return Task.FromResult(false);
        }

        var content = _publishedContentCache.GetById(umbracoContext.InPreviewMode, key.Value);
        if(content is null) {
            _logger.LogWarning("Content not found for key {Key}", key);
            return Task.FromResult(false);
        }

        request.SetPublishedContent(content);
        httpContext.SetCurrentPage(page.Value);
        return Task.FromResult(true);
    }

    [GeneratedRegex(@"^page-(\d+)$", RegexOptions.IgnoreCase & RegexOptions.CultureInvariant)]
    private partial Regex _pagePathRegex { get; }

    internal int? ParsePagePath(string pagePath) {
        var match = _pagePathRegex.Match(pagePath);
        if(!match.Success) {
            return null;
        }

        if(int.TryParse(match.Groups[1].Value, out var page)) {
            return page;
        }

        return null;
    }
}
