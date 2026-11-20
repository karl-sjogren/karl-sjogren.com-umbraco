using Shorthand.UmbracoHelpers;
using Umbraco.Cms.Core.Models.PublishedContent;
using UmbracoBlog.Core.Contracts;
using UmbracoBlog.Core.Documents;
using UmbracoBlog.Core.ViewModels;

namespace UmbracoBlog.Core.Services;

public class LayoutViewModelFactory : ILayoutViewModelFactory {
    private readonly IFriendlyPublishedContentHelper _friendlyContentHelper;
    private readonly ILogger<LayoutViewModelFactory> _logger;

    public LayoutViewModelFactory(IFriendlyPublishedContentHelper friendlyContentHelper, ILogger<LayoutViewModelFactory> logger) {
        _friendlyContentHelper = friendlyContentHelper;
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
            CanonicalUrl = _friendlyContentHelper.Url(publishedContent, null, UrlMode.Absolute)
        };
    }
}
