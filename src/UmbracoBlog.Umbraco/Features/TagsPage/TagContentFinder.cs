using Microsoft.Extensions.Options;
using Umbraco.Cms.Core.Configuration.Models;
using Umbraco.Cms.Core.PublishedCache;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Web;

namespace UmbracoBlog.Umbraco.Features.TagsPage;

public class TagContentFinder : IContentFinder {
    internal const string TagRouteKey = "UmbracoBlog.TagsPage.Tag";

    private readonly IUmbracoContextAccessor _umbracoContextAccessor;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IDocumentUrlService _documentUrlService;
    private readonly IPublishedContentCache _publishedContentCache;
    private readonly IOptionsMonitor<WebRoutingSettings> _routingSettings;

    public TagContentFinder(
            IUmbracoContextAccessor umbracoContextAccessor,
            IHttpContextAccessor httpContextAccessor,
            IDocumentUrlService documentUrlService,
            IPublishedContentCache publishedContentCache,
            IOptionsMonitor<WebRoutingSettings> routingSettings) {
        _umbracoContextAccessor = umbracoContextAccessor;
        _httpContextAccessor = httpContextAccessor;
        _documentUrlService = documentUrlService;
        _publishedContentCache = publishedContentCache;
        _routingSettings = routingSettings;
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
        if(lastSlash < 0 || lastSlash == path.Length - 1) {
            return Task.FromResult(false);
        }

        var tag = path[(lastSlash + 1)..];
        var parentPath = lastSlash == 0 ? "/" : path[..lastSlash];
        var key = _documentUrlService.GetDocumentKeyByRoute(
            parentPath, request.Culture, request.Domain?.ContentId, umbracoContext.InPreviewMode);
        if(key == null) {
            return Task.FromResult(false);
        }

        var content = _publishedContentCache.GetById(umbracoContext.InPreviewMode, key.Value);
        if(content?.ContentType.Alias != Core.Documents.TagsPage.ModelTypeAlias) {
            return Task.FromResult(false);
        }

        request.SetPublishedContent(content);
        httpContext.Items[TagRouteKey] = tag;
        return Task.FromResult(true);
    }
}
