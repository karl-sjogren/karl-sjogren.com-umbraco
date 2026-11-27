using Shorthand.UmbracoHelpers;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Services.Navigation;
using Umbraco.Cms.Core.Web;
using UmbracoBlog.Core.Contracts;
using UmbracoBlog.Core.Documents;
using UmbracoBlog.Core.Documents.Composites;
using UmbracoBlog.Core.Elements;
using UmbracoBlog.Core.ViewModels;

namespace UmbracoBlog.Core.Services;

public class LayoutViewModelFactory : ILayoutViewModelFactory {
    private readonly IFriendlyPublishedContentHelper _friendlyContentHelper;
    private readonly IDocumentNavigationQueryService _documentNavigationQueryService;
    private readonly IUmbracoContextFactory _umbracoContextFactory;
    private readonly ILogger<LayoutViewModelFactory> _logger;
    private readonly TimeProvider _timeProvider;

    public LayoutViewModelFactory(
            IFriendlyPublishedContentHelper friendlyContentHelper,
            IDocumentNavigationQueryService documentNavigationQueryService,
            IUmbracoContextFactory umbracoContextFactory,
            TimeProvider timeProvider,
            ILogger<LayoutViewModelFactory> logger) {
        _friendlyContentHelper = friendlyContentHelper;
        _documentNavigationQueryService = documentNavigationQueryService;
        _umbracoContextFactory = umbracoContextFactory;
        _timeProvider = timeProvider;
        _logger = logger;
    }

    public LayoutViewModel<TViewModel> Create<TViewModel>(TViewModel viewModel, IPublishedContent publishedContent) where TViewModel : class {
        string? seoTitle = null;
        string? seoDescription = null;
        var blockSearchEngineIndexing = false;

        if(publishedContent is ISeoComposite seoComposite) {
            seoTitle = seoComposite.SeoTitle;
            seoDescription = seoComposite.SeoDescription;
            blockSearchEngineIndexing = seoComposite.BlockSearchEngineIndexing;
        }

        return new LayoutViewModel<TViewModel>(viewModel) {
            MetaTitle = publishedContent.Name,
            SeoTitle = seoTitle,
            MetaDescription = seoDescription,
            BlockSearchEngineIndexing = blockSearchEngineIndexing,
            Url = _friendlyContentHelper.Url(publishedContent),
            CanonicalUrl = _friendlyContentHelper.Url(publishedContent, null, UrlMode.Absolute),
            SiteSettings = GetSiteSettings(publishedContent),
            CopyrightYear = _timeProvider.GetUtcNow().Year
        };
    }

    private SiteSettingsElement? GetSiteSettings(IPublishedContent publishedContent) {
        var startPage = GetStartPage(publishedContent);
        if(startPage is null) {
            return null;
        }

        return startPage.SiteSettings?.OfType<SiteSettingsElement>().SingleOrDefault();
    }

    private StartPage? GetStartPage(IPublishedContent publishedContent) {
        var startPage = _friendlyContentHelper.AncestorsOrSelf(publishedContent).OfType<StartPage>().FirstOrDefault();
        if(startPage is null) {
            _logger.LogWarning("Start page not found for published content with ID {PublishedContentId}", publishedContent?.Id);
        }

        return startPage;
    }
}
