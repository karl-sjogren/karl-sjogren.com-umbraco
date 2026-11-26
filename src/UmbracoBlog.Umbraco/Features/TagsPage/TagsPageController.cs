using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Shorthand.UmbracoHelpers;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Search.Core.Models.Searching.Faceting;
using Umbraco.Cms.Search.Core.Models.Searching.Filtering;
using Umbraco.Cms.Search.Core.Models.Searching.Sorting;
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

        if(HttpContext.GetTagSlug() is string tagSlug) {
            return await TagDetailsAsync(tagsPage, tagSlug);
        }

        var tags = await GetTagsAsync(cancellationToken);

        var viewModel = new TagsPageViewModel(tagsPage, tags);

        var layoutViewModel = _layoutViewModelFactory.Create(viewModel, tagsPage);
        return View(layoutViewModel);
    }

    [NonAction]
    public async Task<IActionResult> TagDetailsAsync(Core.Documents.TagsPage tagsPage, string tag) {
        var blogPosts = await GetBlogPostsByTagAsync(tag);

        var viewModel = new TagDetailsViewModel(tagsPage, tag, blogPosts);

        var layoutViewModel = _layoutViewModelFactory.Create(viewModel, tagsPage);
        return View("Details", layoutViewModel);
    }

    public async Task<ICollection<TagsPageHitViewModel>> GetTagsAsync(CancellationToken cancellationToken) {
        var results = await _searcher.SearchAsync(
            indexAlias: UmbracoSearchConstants.IndexAliases.PublishedContent,
            filters: [
                new KeywordFilter(
                    FieldName: "UmbracoBlog_ContentTypeAlias",
                    Values: [Core.Documents.BlogPost.ModelTypeAlias],
                    Negate: false
                )
            ],
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

    public async Task<ICollection<TagsDetailsBlogPostViewModel>> GetBlogPostsByTagAsync(string tag) {
        var results = await _searcher.SearchAsync(
            indexAlias: UmbracoSearchConstants.IndexAliases.PublishedContent,
            filters: [
                new KeywordFilter(
                    FieldName: Core.Documents.BlogPost.PropertyAliases.Tags,
                    Values: [tag],
                    Negate: false
                ),
                new KeywordFilter(
                    FieldName: "UmbracoBlog_ContentTypeAlias",
                    Values: [Core.Documents.BlogPost.ModelTypeAlias],
                    Negate: false
                )
            ],
            sorters: [
                new DateTimeOffsetSorter(
                    FieldName: Core.Documents.BlogPost.PropertyAliases.PublishDate,
                    Direction: Direction.Descending
                )
            ]
        );

        var documents = results.Documents.Select(x => _publishedContentQuery.Content(x.Id));

        var hits = documents
            .OfType<Core.Documents.BlogPost>()
            .Select(doc => new TagsDetailsBlogPostViewModel(
                Title: doc.Name,
                PublishDate: doc.PublishDate,
                Url: _friendlyPublishedContentHelper.Url(doc)
            ))
            .ToList();

        return hits;
    }
}
