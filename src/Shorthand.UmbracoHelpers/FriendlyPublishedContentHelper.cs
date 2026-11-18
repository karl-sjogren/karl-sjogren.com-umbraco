using Examine;
using Microsoft.Extensions.Options;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Configuration.Models;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PublishedCache;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Services.Navigation;
using Umbraco.Cms.Core.Web;

namespace Shorthand.UmbracoHelpers;

public class FriendlyPublishedContentHelper : IFriendlyPublishedContentHelper {
    private readonly IVariationContextAccessor _variationContextAccessor;
    private readonly IDomainCache _domainCache;
    private readonly IDocumentNavigationQueryService _documentNavigationQueryService;
    private readonly IMediaNavigationQueryService _mediaNavigationQueryService;
    private readonly IPublishedContentStatusFilteringService _publishedContentStatusFilteringService;
    private readonly IPublishedMediaStatusFilteringService _publishedMediaStatusFilteringService;
    private readonly IPublishedModelFactory _publishedModelFactory;
    private readonly IPublishedUrlProvider _publishedUrlProvider;
    private readonly IUmbracoContextAccessor _umbracoContextAccessor;
    private readonly ISiteDomainMapper _siteDomainMapper;
    private readonly ITemplateService _templateService;
    private readonly IOptions<WebRoutingSettings> _webRoutingSettings;
    private readonly IContentTypeService _contentTypeService;
    private readonly IPublishedValueFallback _publishedValueFallback;
    private readonly IDocumentUrlService _documentUrlService;
    private readonly IUserService _userService;
    private readonly IExamineManager _examineManager;

    public FriendlyPublishedContentHelper(
            IVariationContextAccessor variationContextAccessor,
            IDomainCache domainCache,
            IDocumentNavigationQueryService documentNavigationQueryService,
            IMediaNavigationQueryService mediaNavigationQueryService,
            IPublishedContentStatusFilteringService publishedContentStatusFilteringService,
            IPublishedMediaStatusFilteringService publishedMediaStatusFilteringService,
            IPublishedModelFactory publishedModelFactory,
            IPublishedUrlProvider publishedUrlProvider,
            IUmbracoContextAccessor umbracoContextAccessor,
            ISiteDomainMapper siteDomainMapper,
            ITemplateService templateService,
            IOptions<WebRoutingSettings> webRoutingSettings,
            IContentTypeService contentTypeService,
            IPublishedValueFallback publishedValueFallback,
            IDocumentUrlService documentUrlService,
            IUserService userService,
            IExamineManager examineManager) {
        _variationContextAccessor = variationContextAccessor;
        _domainCache = domainCache;
        _documentNavigationQueryService = documentNavigationQueryService;
        _mediaNavigationQueryService = mediaNavigationQueryService;
        _publishedContentStatusFilteringService = publishedContentStatusFilteringService;
        _publishedMediaStatusFilteringService = publishedMediaStatusFilteringService;
        _publishedModelFactory = publishedModelFactory;
        _publishedUrlProvider = publishedUrlProvider;
        _umbracoContextAccessor = umbracoContextAccessor;
        _siteDomainMapper = siteDomainMapper;
        _templateService = templateService;
        _webRoutingSettings = webRoutingSettings;
        _contentTypeService = contentTypeService;
        _publishedValueFallback = publishedValueFallback;
        _documentUrlService = documentUrlService;
        _userService = userService;
        _examineManager = examineManager;
    }

    public IPublishedContent? CreateModel(
        IPublishedContent content)
        => content.CreateModel(_publishedModelFactory);

    public string Name(
        IPublishedContent content,
        string? culture = null)
        => ((IPublishedElement)content).Name(_variationContextAccessor, culture);

    public string? UrlSegment(
        IPublishedContent content,
        string? culture = null) {
        ArgumentNullException.ThrowIfNull(content);

        var resolvedCulture = culture ?? _variationContextAccessor.VariationContext?.Culture ?? string.Empty;
        var isDraft = _umbracoContextAccessor.TryGetUmbracoContext(out IUmbracoContext? umbracoContext) && umbracoContext.InPreviewMode;
        return _documentUrlService.GetUrlSegment(content.Key, resolvedCulture, isDraft);
    }

    public DateTime CultureDate(
        IPublishedContent content,
        string? culture = null)
        => ((IPublishedElement)content).CultureDate(_variationContextAccessor, culture);

    public string GetTemplateAlias(IPublishedContent content)
        => content.GetTemplateAlias(_templateService);

    public bool IsAllowedTemplate(IPublishedContent content, int templateId)
        => content.IsAllowedTemplate(_contentTypeService, _webRoutingSettings.Value, templateId);

    public bool IsAllowedTemplate(IPublishedContent content, string templateAlias)
        => content.IsAllowedTemplate(
            _templateService,
            _contentTypeService,
            _webRoutingSettings.Value.DisableAlternativeTemplates,
            _webRoutingSettings.Value.ValidateAlternativeTemplates,
            templateAlias);

    public bool IsAllowedTemplate(
        IPublishedContent content,
        bool disableAlternativeTemplates,
        bool validateAlternativeTemplates,
        int templateId)
        => content.IsAllowedTemplate(
            _contentTypeService,
            disableAlternativeTemplates,
            validateAlternativeTemplates,
            templateId);

    public bool IsAllowedTemplate(
        IPublishedContent content,
        bool disableAlternativeTemplates,
        bool validateAlternativeTemplates,
        string templateAlias)
        => content.IsAllowedTemplate(
            _templateService,
            _contentTypeService,
            disableAlternativeTemplates,
            validateAlternativeTemplates,
            templateAlias);

    public bool HasValue(
        IPublishedContent content,
        string alias,
        string? culture = null,
        string? segment = null,
        Fallback fallback = default)
        =>
            content.HasValue(_publishedValueFallback, alias, culture, segment, fallback);

    public object? Value(IPublishedContent content, string alias, string? culture = null, string? segment = null, Fallback fallback = default, object? defaultValue = default)
        => content.Value(_publishedValueFallback, alias, culture, segment, fallback, defaultValue);

    public T? Value<T>(IPublishedContent content, string alias, string? culture = null, string? segment = null, Fallback fallback = default, T? defaultValue = default)
        => content.Value(_publishedValueFallback, alias, culture, segment, fallback, defaultValue);

    public IPublishedContent Root(IPublishedContent content)
        => content.Root(GetNavigationQueryService(content), GetPublishedStatusFilteringService(content));

    public T? Root<T>(IPublishedContent content)
        where T : class, IPublishedContent
        => content.Root<T>(GetNavigationQueryService(content), GetPublishedStatusFilteringService(content));

    public T? Parent<T>(IPublishedContent content)
        where T : class, IPublishedContent
        => content.Parent<T>(GetNavigationQueryService(content), GetPublishedStatusFilteringService(content));

    public IPublishedContent? Parent(IPublishedContent content)
        => content.Parent<IPublishedContent>(GetNavigationQueryService(content), GetPublishedStatusFilteringService(content));

    public IEnumerable<IPublishedContent> Ancestors(IPublishedContent content)
        => content.Ancestors(GetNavigationQueryService(content), GetPublishedStatusFilteringService(content));

    public IEnumerable<IPublishedContent> AncestorsOrSelf(IPublishedContent content)
        => content.AncestorsOrSelf(GetNavigationQueryService(content), GetPublishedStatusFilteringService(content));

    public IEnumerable<T> AncestorsOrSelf<T>(IPublishedContent content)
        where T : class, IPublishedContent
        => content.AncestorsOrSelf<T>(GetNavigationQueryService(content), GetPublishedStatusFilteringService(content));

    public IPublishedContent? Ancestor(IPublishedContent content)
        => content.Ancestor(GetNavigationQueryService(content), GetPublishedStatusFilteringService(content));

    public T? Ancestor<T>(IPublishedContent content)
        where T : class, IPublishedContent
        => content.Ancestor<T>(GetNavigationQueryService(content), GetPublishedStatusFilteringService(content));

    public T? AncestorOrSelf<T>(IPublishedContent content)
        where T : class, IPublishedContent
        => content.AncestorOrSelf<T>(GetNavigationQueryService(content), GetPublishedStatusFilteringService(content));

    public IEnumerable<IPublishedContent> DescendantsOrSelfOfType(
        IEnumerable<IPublishedContent> parentNodes, string docTypeAlias, string? culture = null) {
        IPublishedContent[] parentNodesAsArray = parentNodes as IPublishedContent[] ?? parentNodes.ToArray();
        if(parentNodesAsArray.Length == 0) {
            return [];
        }

        return parentNodesAsArray.DescendantsOrSelfOfType(
            GetNavigationQueryService(parentNodesAsArray.First()),
            GetPublishedStatusFilteringService(parentNodesAsArray.First()),
            docTypeAlias,
            culture);
    }

    public IEnumerable<T> DescendantsOrSelf<T>(
        IEnumerable<IPublishedContent> parentNodes,
        string? culture = null)
        where T : class, IPublishedContent {
        IPublishedContent[] parentNodesAsArray = parentNodes as IPublishedContent[] ?? parentNodes.ToArray();
        if(parentNodesAsArray.Length == 0) {
            return [];
        }

        return parentNodesAsArray.DescendantsOrSelf<T>(
            GetNavigationQueryService(parentNodesAsArray.First()),
            GetPublishedStatusFilteringService(parentNodesAsArray.First()),
            culture);
    }

    public IEnumerable<IPublishedContent> Descendants(IPublishedContent content, string? culture = null)
        => content.Descendants(GetNavigationQueryService(content), GetPublishedStatusFilteringService(content), culture);

    public IEnumerable<IPublishedContent> Descendants(IPublishedContent content, int level, string? culture = null)
        => content.Descendants(GetNavigationQueryService(content), GetPublishedStatusFilteringService(content), level, culture);

    public IEnumerable<IPublishedContent> DescendantsOfType(IPublishedContent content, string contentTypeAlias, string? culture = null)
        => content.DescendantsOfType(GetNavigationQueryService(content), GetPublishedStatusFilteringService(content), contentTypeAlias, culture);

    public IEnumerable<T> Descendants<T>(IPublishedContent content, string? culture = null)
        where T : class, IPublishedContent
        => content.Descendants<T>(GetNavigationQueryService(content), GetPublishedStatusFilteringService(content), culture);

    public IEnumerable<T> Descendants<T>(IPublishedContent content, int level, string? culture = null)
        where T : class, IPublishedContent
        => content.Descendants<T>(GetNavigationQueryService(content), GetPublishedStatusFilteringService(content), level, culture);

    public IEnumerable<IPublishedContent> DescendantsOrSelf(
        IPublishedContent content,
        string? culture = null)
        => content.DescendantsOrSelf(GetNavigationQueryService(content), GetPublishedStatusFilteringService(content), culture);

    public IEnumerable<IPublishedContent> DescendantsOrSelf(IPublishedContent content, int level, string? culture = null)
        => content.DescendantsOrSelf(GetNavigationQueryService(content), GetPublishedStatusFilteringService(content), level, culture);

    public IEnumerable<IPublishedContent> DescendantsOrSelfOfType(IPublishedContent content, string contentTypeAlias, string? culture = null)
        => content.DescendantsOrSelfOfType(GetNavigationQueryService(content), GetPublishedStatusFilteringService(content), contentTypeAlias, culture);

    public IEnumerable<T> DescendantsOrSelf<T>(IPublishedContent content, string? culture = null)
        where T : class, IPublishedContent
        => content.DescendantsOrSelf<T>(GetNavigationQueryService(content), GetPublishedStatusFilteringService(content), culture);

    public IEnumerable<T> DescendantsOrSelf<T>(IPublishedContent content, int level, string? culture = null)
        where T : class, IPublishedContent
        => content.DescendantsOrSelf<T>(GetNavigationQueryService(content), GetPublishedStatusFilteringService(content), level, culture);

    public IPublishedContent? Descendant(IPublishedContent content, string? culture = null)
        => content.Descendant(GetNavigationQueryService(content), GetPublishedStatusFilteringService(content), culture);

    public IPublishedContent? Descendant(IPublishedContent content, int level, string? culture = null)
        => content.Descendant(GetNavigationQueryService(content), GetPublishedStatusFilteringService(content), level, culture);

    public IPublishedContent? DescendantOfType(IPublishedContent content, string contentTypeAlias, string? culture = null)
        => content.DescendantOfType(GetNavigationQueryService(content), GetPublishedStatusFilteringService(content), contentTypeAlias, culture);

    public T? Descendant<T>(IPublishedContent content, string? culture = null)
        where T : class, IPublishedContent
        => content.Descendant<T>(GetNavigationQueryService(content), GetPublishedStatusFilteringService(content), culture);

    public T? Descendant<T>(IPublishedContent content, int level, string? culture = null)
        where T : class, IPublishedContent
        => content.Descendant<T>(GetNavigationQueryService(content), GetPublishedStatusFilteringService(content), level, culture);

    public IPublishedContent DescendantOrSelf(IPublishedContent content, string? culture = null)
        => content.DescendantOrSelf(GetNavigationQueryService(content), GetPublishedStatusFilteringService(content), culture);

    public IPublishedContent? DescendantOrSelf(IPublishedContent content, int level, string? culture = null)
        => content.DescendantOrSelf(GetNavigationQueryService(content), GetPublishedStatusFilteringService(content), level, culture);

    public IPublishedContent? DescendantOrSelfOfType(IPublishedContent content, string contentTypeAlias, string? culture = null)
        => content.DescendantOrSelfOfType(GetNavigationQueryService(content), GetPublishedStatusFilteringService(content), contentTypeAlias, culture);

    public T? DescendantOrSelf<T>(IPublishedContent content, string? culture = null)
        where T : class, IPublishedContent
        => content.DescendantOrSelf<T>(GetNavigationQueryService(content), GetPublishedStatusFilteringService(content), culture);

    public T? DescendantOrSelf<T>(IPublishedContent content, int level, string? culture = null)
        where T : class, IPublishedContent
        => content.DescendantOrSelf<T>(GetNavigationQueryService(content), GetPublishedStatusFilteringService(content), level, culture);

    public IEnumerable<IPublishedContent> Children(IPublishedContent content, string? culture = null)
        => content.Children(GetNavigationQueryService(content), GetPublishedStatusFilteringService(content), culture);

    public IEnumerable<IPublishedContent> Children(
        IPublishedContent content,
        Func<IPublishedContent, bool> predicate,
        string? culture = null)
        => content.Children(GetNavigationQueryService(content), GetPublishedStatusFilteringService(content), predicate, culture);

    public IEnumerable<IPublishedContent> ChildrenOfType(IPublishedContent content, string contentTypeAlias, string? culture = null)
        => content.ChildrenOfType(GetNavigationQueryService(content), GetPublishedStatusFilteringService(content), contentTypeAlias, culture);

    public IEnumerable<T> Children<T>(IPublishedContent content, string? culture = null)
        where T : class, IPublishedContent
        => content.Children<T>(GetNavigationQueryService(content), GetPublishedStatusFilteringService(content), culture);

    public IPublishedContent? FirstChild(IPublishedContent content, string? culture = null)
        => content.FirstChild(GetNavigationQueryService(content), GetPublishedStatusFilteringService(content), culture);

    public IPublishedContent? FirstChildOfType(IPublishedContent content, string contentTypeAlias, string? culture = null)
        => content.FirstChildOfType(GetNavigationQueryService(content), GetPublishedStatusFilteringService(content), contentTypeAlias, culture);

    public IPublishedContent? FirstChild(IPublishedContent content, Func<IPublishedContent, bool> predicate, string? culture = null)
        => content.FirstChild(GetNavigationQueryService(content), GetPublishedStatusFilteringService(content), predicate, culture);

    public IPublishedContent? FirstChild(IPublishedContent content, Guid uniqueId, string? culture = null)
        => content.FirstChild(GetNavigationQueryService(content), GetPublishedStatusFilteringService(content), uniqueId, culture);

    public T? FirstChild<T>(IPublishedContent content, string? culture = null)
        where T : class, IPublishedContent
        => content.FirstChild<T>(GetNavigationQueryService(content), GetPublishedStatusFilteringService(content), culture);

    public T? FirstChild<T>(IPublishedContent content, Func<T, bool> predicate, string? culture = null)
        where T : class, IPublishedContent
        => content.FirstChild(GetNavigationQueryService(content), GetPublishedStatusFilteringService(content), predicate, culture);

    public IEnumerable<IPublishedContent> Siblings(IPublishedContent content, string? culture = null)
        => content.Siblings(GetNavigationQueryService(content), GetPublishedStatusFilteringService(content), culture);

    public IEnumerable<IPublishedContent> SiblingsOfType(IPublishedContent content, string contentTypeAlias, string? culture = null)
        => content.SiblingsOfType(GetNavigationQueryService(content), GetPublishedStatusFilteringService(content), contentTypeAlias, culture);

    public IEnumerable<T> Siblings<T>(IPublishedContent content, string? culture = null)
        where T : class, IPublishedContent
        => content.Siblings<T>(GetNavigationQueryService(content), GetPublishedStatusFilteringService(content), culture);

    public IEnumerable<IPublishedContent>? SiblingsAndSelf(
        IPublishedContent content,
        string? culture = null)
        => content.SiblingsAndSelf(GetNavigationQueryService(content), GetPublishedStatusFilteringService(content), culture);

    public IEnumerable<IPublishedContent> SiblingsAndSelfOfType(
        IPublishedContent content,
        string contentTypeAlias,
        string? culture = null)
        => content.SiblingsAndSelfOfType(GetNavigationQueryService(content), GetPublishedStatusFilteringService(content), contentTypeAlias, culture);

    public IEnumerable<T> SiblingsAndSelf<T>(IPublishedContent content, string? culture = null)
        where T : class, IPublishedContent
        => content.SiblingsAndSelf<T>(GetNavigationQueryService(content), GetPublishedStatusFilteringService(content), culture);

    public string Url(IPublishedContent content, string? culture = null, UrlMode mode = UrlMode.Default)
        => content.Url(_publishedUrlProvider, culture, mode);

    public string MediaUrl(
        IPublishedContent content,
        string? culture = null,
        UrlMode mode = UrlMode.Default,
        string propertyAlias = Constants.Conventions.Media.File)
        => content.MediaUrl(_publishedUrlProvider, culture, mode, propertyAlias);

    public string? CreatorName(IPublishedContent content)
        =>
        ((IPublishedElement)content).CreatorName(_userService);

    public string? WriterName(IPublishedContent content)
        =>
        ((IPublishedElement)content).WriterName(_userService);

    public string? GetCultureFromDomains(
        IPublishedContent content,
        Uri? current = null)
        => content.GetCultureFromDomains(_umbracoContextAccessor, _siteDomainMapper, _domainCache, _documentNavigationQueryService, GetPublishedStatusFilteringService(content), current);

    public IEnumerable<PublishedSearchResult> SearchDescendants(IPublishedContent content, string term, string? indexName = null)
        => content.SearchDescendants(_examineManager, _umbracoContextAccessor, term, indexName);

    public IEnumerable<PublishedSearchResult> SearchChildren(IPublishedContent content, string term, string? indexName = null)
        => content.SearchChildren(_examineManager, _umbracoContextAccessor, term, indexName);

    private INavigationQueryService GetNavigationQueryService(IPublishedContent content) =>
        content.ContentType.ItemType switch {
            PublishedItemType.Content => _documentNavigationQueryService,
            PublishedItemType.Media => _mediaNavigationQueryService,
            _ => throw new NotSupportedException("Unsupported content type."),
        };

    private IPublishedStatusFilteringService GetPublishedStatusFilteringService(IPublishedContent content) =>
        content.ContentType.ItemType switch {
            PublishedItemType.Content => _publishedContentStatusFilteringService,
            PublishedItemType.Media => _publishedMediaStatusFilteringService,
            _ => throw new NotSupportedException("Unsupported content type."),
        };
}
