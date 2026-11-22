using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Shorthand.UmbracoHelpers;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Web.Common.Controllers;
using UmbracoBlog.Core.Contracts;
using UmbracoBlog.Core.Documents;

namespace UmbracoBlog.Umbraco.Features.BlogArchive;

public class BlogArchiveController : RenderController {
    private readonly IFriendlyPublishedContentHelper _friendlyPublishedContentHelper;
    private readonly ILayoutViewModelFactory _layoutViewModelFactory;
    private readonly ILogger<BlogArchiveController> _logger;

    public BlogArchiveController(
            IFriendlyPublishedContentHelper friendlyPublishedContentHelper,
            ILayoutViewModelFactory layoutViewModelFactory,
            ICompositeViewEngine compositeViewEngine,
            IUmbracoContextAccessor umbracoContextAccessor,
            ILogger<BlogArchiveController> logger)
        : base(logger, compositeViewEngine, umbracoContextAccessor) {
        _friendlyPublishedContentHelper = friendlyPublishedContentHelper;
        _layoutViewModelFactory = layoutViewModelFactory;
        _logger = logger;
    }

    public override IActionResult Index() {
        var blogArchive = CurrentPage as Core.Documents.BlogArchive;
        if(blogArchive == null) {
            return NotFound();
        }

        var blogArchiveViewModel = new BlogArchiveViewModel(blogArchive, []);

        var startPage = GetStartPage(blogArchive);
        if(startPage is not null) {
            var blogPosts = GetBlogPosts(startPage);

            var groupedBlogPosts = blogPosts
                .Select(x => new BlogArchivePostViewModel(x.MainHeading!, x.PublishDate, _friendlyPublishedContentHelper.Url(x)))
                .OrderByDescending(x => x.Date)
                .GroupBy(x => x.Date.Year)
                .Select(group => new BlogArchiveYearViewModel(group.Key, [.. group]))
                .ToList();

            blogArchiveViewModel = new BlogArchiveViewModel(blogArchive, groupedBlogPosts);
        }

        var layoutViewModel = _layoutViewModelFactory.Create(blogArchiveViewModel, blogArchive);
        return View(layoutViewModel);
    }

    private Core.Documents.StartPage? GetStartPage(IPublishedContent publishedContent) {
        var startPage = _friendlyPublishedContentHelper.AncestorsOrSelf(publishedContent).OfType<Core.Documents.StartPage>().FirstOrDefault();
        if(startPage is null) {
            _logger.LogWarning("Start page not found for published content with ID {PublishedContentId}", publishedContent?.Id);
        }

        return startPage;
    }

    private ICollection<Core.Documents.BlogPost> GetBlogPosts(Core.Documents.StartPage startPage) {
        var blogIndex = _friendlyPublishedContentHelper
            .ChildrenOfType(startPage, BlogIndex.ModelTypeAlias)
            .FirstOrDefault();

        if(blogIndex == null) {
            _logger.LogWarning("No BlogIndex found for StartPage with ID {StartPageId}", startPage.Id);
            return [];
        }

        var blogPosts = _friendlyPublishedContentHelper
            .ChildrenOfType(blogIndex, Core.Documents.BlogPost.ModelTypeAlias)
            .OfType<Core.Documents.BlogPost>()
            .ToList();

        return [.. blogPosts];
    }
}
