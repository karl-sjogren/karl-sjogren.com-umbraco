using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Shorthand.UmbracoHelpers;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Search.Core.Models.Searching.Faceting;
using Umbraco.Cms.Search.Core.Services;
using Umbraco.Cms.Web.Common.Controllers;
using UmbracoBlog.Core.Contracts;

namespace UmbracoBlog.Umbraco.Features.TagsPage;

public class TagsPageController : RenderController {
    private readonly ISearcher _searcher;
    private readonly IPublishedContentQuery _publishedContentQuery;
    private readonly IFriendlyPublishedContentHelper _friendlyPublishedContentHelper;
    private readonly ILayoutViewModelFactory _layoutViewModelFactory;
    private readonly ILogger<TagsPageController> _logger;

    public TagsPageController(
            ISearcher searcher,
            IPublishedContentQuery publishedContentQuery,
            IFriendlyPublishedContentHelper friendlyPublishedContentHelper,
            ILayoutViewModelFactory layoutViewModelFactory,
            ICompositeViewEngine compositeViewEngine,
            IUmbracoContextAccessor umbracoContextAccessor,
            ILogger<TagsPageController> logger)
        : base(logger, compositeViewEngine, umbracoContextAccessor) {
        _searcher = searcher;
        _publishedContentQuery = publishedContentQuery;
        _friendlyPublishedContentHelper = friendlyPublishedContentHelper;
        _layoutViewModelFactory = layoutViewModelFactory;
        _logger = logger;
    }

    [NonAction]
    public override IActionResult Index() => throw new NotImplementedException("Use IndexAsync instead.");

    public async Task<IActionResult> IndexAsync(CancellationToken cancellationToken) {
        var tagsPage = CurrentPage as Core.Documents.TagsPage;
        if(tagsPage == null) {
            return NotFound();
        }

        var tags = await GetTagsAsync(cancellationToken);

        var viewModel = new TagsPageViewModel(tagsPage, tags);

        var layoutViewModel = _layoutViewModelFactory.Create(viewModel, tagsPage);
        return View(layoutViewModel);
    }

    public IActionResult TagDetails(string tag) {
        return View(tag);
    }

    public async Task<ICollection<TagsPageHitViewModel>> GetTagsAsync(CancellationToken cancellationToken) {
        var results = await _searcher.SearchAsync(
            indexAlias: UmbracoSearchConstants.IndexAliases.PublishedContent,
            facets:
            [
                new KeywordFacet(
                    FieldName: Core.Documents.BlogPost.PropertyAliases.Tags
                )
            ]
        );

        var tagsFacet = results.Facets.FirstOrDefault(x => x.FieldName == Core.Documents.BlogPost.PropertyAliases.Tags);

        return tagsFacet?
            .Values
            .OfType<KeywordFacetValue>()
            .Select(x => new TagsPageHitViewModel(x.Key, x.Count))
            .OrderByDescending(x => x.Count)
            .ToList() ?? [];
    }
}
