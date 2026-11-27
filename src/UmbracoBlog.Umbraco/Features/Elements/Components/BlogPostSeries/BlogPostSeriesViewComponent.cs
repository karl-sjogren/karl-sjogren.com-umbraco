using Microsoft.AspNetCore.Mvc;
using Shorthand.UmbracoHelpers;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Search.Core.Extensions;
using Umbraco.Cms.Search.Core.Models.Searching.Filtering;
using Umbraco.Cms.Search.Core.Services;

namespace UmbracoBlog.Umbraco.Features.Elements.Components.BlogPostSeries;

public class BlogPostSeriesViewComponent : ViewComponent {
    private readonly ISearcher _searcher;
    private readonly IPublishedContentQuery _publishedContentQuery;
    private readonly IFriendlyPublishedContentHelper _friendlyPublishedContentHelper;
    private readonly ILogger<BlogPostSeriesViewComponent> _logger;

    public BlogPostSeriesViewComponent(
            ISearcher searcher,
            IPublishedContentQuery publishedContentQuery,
            IFriendlyPublishedContentHelper friendlyPublishedContentHelper,
            ILogger<BlogPostSeriesViewComponent> logger) {
        _searcher = searcher;
        _publishedContentQuery = publishedContentQuery;
        _friendlyPublishedContentHelper = friendlyPublishedContentHelper;
        _logger = logger;
    }

    public async Task<IViewComponentResult> InvokeAsync(IPublishedElement element, Core.Documents.BlogPost currentPost) {
        var model = element as Core.Elements.BlogPostSeriesElement;
        if(model is null) {
            throw new ArgumentNullException(nameof(element));
        }

        var results = await _searcher.SearchAsync(
            indexAlias: UmbracoSearchConstants.IndexAliases.PublishedContent,
            filters: [
                new KeywordFilter(
                    FieldName: "UmbracoBlog_PartOfSeries",
                    Values: [model.Key.AsKeyword()],
                    Negate: false
                ),
            ],
            skip: 0,
            take: 100
        );

        var documents = results.Documents.Select(x => _publishedContentQuery.Content(x.Id));
        var hits = documents
            .OfType<IPublishedContent>()
            .Select(x => {
                var content = _publishedContentQuery.Content(x.Id) as Core.Documents.BlogPost;
                if(content == null) {
                    return null;
                }

                var url = _friendlyPublishedContentHelper.Url(content);
                return new BlogPostSeriesLinkViewModel(content.Name, content.PublishDate, url, content.Id == currentPost.Id);
            })
            .OfType<BlogPostSeriesLinkViewModel>()
            .OrderBy(x => x.PublishDate)
            .ToArray();

        var viewModel = new BlogPostSeriesViewModel {
            Title = model.SeriesName ?? model.Name,
            Parts = hits
        };

        return View(viewModel);
    }
}
