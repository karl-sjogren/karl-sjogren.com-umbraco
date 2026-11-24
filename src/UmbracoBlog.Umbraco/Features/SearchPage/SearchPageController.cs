using Examine;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Shorthand.UmbracoHelpers;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Web.Common.Controllers;
using UmbracoBlog.Core.Contracts;

namespace UmbracoBlog.Umbraco.Features.SearchPage;

public class SearchPageController : RenderController {
    private readonly IExamineManager _examineManager;
    private readonly IPublishedContentQuery _publishedContentQuery;
    private readonly IFriendlyPublishedContentHelper _friendlyPublishedContentHelper;
    private readonly ILayoutViewModelFactory _layoutViewModelFactory;
    private readonly ILogger<SearchPageController> _logger;

    private IReadOnlyDictionary<string, string> _searchableTypeAliases => new Dictionary<string, string> {
        { Core.Documents.ContentPage.ModelTypeAlias, "Content Page" },
        { Core.Documents.BlogPost.ModelTypeAlias, "Blog Post" }
    }.AsReadOnly();

    public SearchPageController(
            IExamineManager examineManager,
            IPublishedContentQuery publishedContentQuery,
            IFriendlyPublishedContentHelper friendlyPublishedContentHelper,
            ILayoutViewModelFactory layoutViewModelFactory,
            ICompositeViewEngine compositeViewEngine,
            IUmbracoContextAccessor umbracoContextAccessor,
            ILogger<SearchPageController> logger)
        : base(logger, compositeViewEngine, umbracoContextAccessor) {
        _examineManager = examineManager;
        _publishedContentQuery = publishedContentQuery;
        _friendlyPublishedContentHelper = friendlyPublishedContentHelper;
        _layoutViewModelFactory = layoutViewModelFactory;
        _logger = logger;
    }

    [NonAction]
    public override IActionResult Index() => throw new NotImplementedException("Use the Index action with searchQuery and pageIndex parameters.");

    public IActionResult Index([FromQuery] string searchQuery, [FromQuery] int pageIndex = 0) {
        var searchPage = CurrentPage as Core.Documents.SearchPage;
        if(searchPage == null) {
            return NotFound();
        }

        var searchResults = GetSearchResults(searchQuery, pageIndex, 10, out var totalHits);

        var viewModel = new SearchPageViewModel(searchPage, searchQuery, searchResults, totalHits);

        var layoutViewModel = _layoutViewModelFactory.Create(viewModel, searchPage);
        layoutViewModel.HideSidebarSearch = true; // or true depending on your logic
        return View(layoutViewModel);
    }

    public ICollection<SearchPageHitViewModel> GetSearchResults(string searchQuery, int pageIndex, int pageSize, out long totalHits) {
        if(!_examineManager.TryGetIndex(UmbracoConstants.UmbracoIndexes.ExternalIndexName, out var index)) {
            throw new InvalidOperationException("Failed to get Examine index.");
        }

        if(string.IsNullOrEmpty(searchQuery)) {
            totalHits = 0;
            return [];
        }

        var results = index
            .Searcher
            .CreateQuery("content")
            .GroupedOr(["__NodeTypeAlias"], _searchableTypeAliases.Keys.ToArray())
            .And()
            .GroupedOr(["nodeName", "mainHeading", "mainContent", "tags"], searchQuery)
            .Execute();

        totalHits = results.TotalItemCount;
        var hits = results
            .Skip(pageIndex * pageSize)
            .Take(pageSize)
            .Select(x => new { Id = x.Id, Type = x.Values["__NodeTypeAlias"], Score = x.Score });

        return hits
            .Select(x => {
                var content = _publishedContentQuery.Content(x.Id);
                if(content == null) {
                    return null;
                }

                var url = _friendlyPublishedContentHelper.Url(content);
                var translatedType = _searchableTypeAliases.TryGetValue(x.Type, out var type) ? type : x.Type;
                return new SearchPageHitViewModel(content.Name, url, translatedType, x.Score);
            })
            .OfType<SearchPageHitViewModel>()
            .ToArray();
    }
}
