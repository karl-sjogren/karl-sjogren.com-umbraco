using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Shorthand.UmbracoHelpers;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Search.Core.Models.Searching.Filtering;
using Umbraco.Cms.Search.Core.Services;
using Umbraco.Cms.Web.Common.Controllers;
using UmbracoBlog.Core.Contracts;
using UmbracoBlog.Umbraco.Features.Pagination;

namespace UmbracoBlog.Umbraco.Features.SearchPage;

public class SearchPageController : RenderController {
    private readonly ISearcher _searcher;
    private readonly IPublishedContentQuery _publishedContentQuery;
    private readonly IFriendlyPublishedContentHelper _friendlyPublishedContentHelper;
    private readonly ILayoutViewModelFactory _layoutViewModelFactory;
    private readonly ILogger<SearchPageController> _logger;

    private IReadOnlyDictionary<string, string> _searchableTypeAliases => new Dictionary<string, string> {
        { Core.Documents.ContentPage.ModelTypeAlias, "Content Page" },
        { Core.Documents.BlogPost.ModelTypeAlias, "Blog Post" }
    }.AsReadOnly();

    public SearchPageController(
            ISearcher searcher,
            IPublishedContentQuery publishedContentQuery,
            IFriendlyPublishedContentHelper friendlyPublishedContentHelper,
            ILayoutViewModelFactory layoutViewModelFactory,
            ICompositeViewEngine compositeViewEngine,
            IUmbracoContextAccessor umbracoContextAccessor,
            ILogger<SearchPageController> logger)
        : base(logger, compositeViewEngine, umbracoContextAccessor) {
        _searcher = searcher;
        _publishedContentQuery = publishedContentQuery;
        _friendlyPublishedContentHelper = friendlyPublishedContentHelper;
        _layoutViewModelFactory = layoutViewModelFactory;
        _logger = logger;
    }

    [NonAction]
    public override IActionResult Index() => throw new NotImplementedException("Use the Index action with searchQuery and pageIndex parameters.");

    public async Task<IActionResult> IndexAsync([FromQuery] string searchQuery) {
        var searchPage = CurrentPage as Core.Documents.SearchPage;
        if(searchPage == null) {
            return NotFound();
        }

        var pageNumber = HttpContext.GetCurrentPage();
        var pageSize = 10;

        var (searchResults, totalHits) = await GetSearchResultsAsync(searchQuery, pageNumber, pageSize);

        var viewModel = new SearchPageViewModel(searchPage, searchQuery, searchResults) {
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalHits = (int)totalHits
        };

        var layoutViewModel = _layoutViewModelFactory.Create(viewModel, searchPage);
        layoutViewModel.HideSidebarSearch = true; // or true depending on your logic
        return View(layoutViewModel);
    }

    public async Task<(ICollection<SearchPageHitViewModel>, long)> GetSearchResultsAsync(string searchQuery, int pageNumber, int pageSize) {
        if(string.IsNullOrEmpty(searchQuery)) {
            return (Array.Empty<SearchPageHitViewModel>(), 0);
        }

        var results = await _searcher.SearchAsync(
            indexAlias: UmbracoSearchConstants.IndexAliases.PublishedContent,
            query: searchQuery,
            filters: [
                new KeywordFilter(
                    FieldName: "UmbracoBlog_ContentTypeAlias",
                    Values: [.. _searchableTypeAliases.Keys],
                    Negate: false
                ),
            ],
            skip: (pageNumber - 1) * pageSize,
            take: pageSize
        );

        var totalHits = results.Total;

        var documents = results.Documents.Select(x => _publishedContentQuery.Content(x.Id));
        var hits = documents
            .OfType<IPublishedContent>()
            .Select(x => {
                var content = _publishedContentQuery.Content(x.Id);
                if(content == null) {
                    return null;
                }

                var url = _friendlyPublishedContentHelper.Url(content);
                var translatedType = _searchableTypeAliases.TryGetValue(x.ContentType.Alias, out var type) ? type : x.ContentType.Alias;
                return new SearchPageHitViewModel(content.Name, url, translatedType);
            })
            .OfType<SearchPageHitViewModel>()
            .ToArray();

        return (hits, totalHits);
    }
}
