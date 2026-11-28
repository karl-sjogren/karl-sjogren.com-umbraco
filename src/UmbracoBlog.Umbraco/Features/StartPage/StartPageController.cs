using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Shorthand.UmbracoHelpers;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Web.Common.Controllers;
using UmbracoBlog.Core.Contracts;
using UmbracoBlog.Core.Documents;
using UmbracoBlog.Umbraco.Features.Pagination;

namespace UmbracoBlog.Umbraco.Features.StartPage;

public class StartPageController : RenderController {
    private readonly IFriendlyPublishedContentHelper _friendlyPublishedContentHelper;
    private readonly ILayoutViewModelFactory _layoutViewModelFactory;
    private readonly ILogger<StartPageController> _logger;

    public StartPageController(
            IFriendlyPublishedContentHelper friendlyPublishedContentHelper,
            ILayoutViewModelFactory layoutViewModelFactory,
            ICompositeViewEngine compositeViewEngine,
            IUmbracoContextAccessor umbracoContextAccessor,
            ILogger<StartPageController> logger)
        : base(logger, compositeViewEngine, umbracoContextAccessor) {
        _friendlyPublishedContentHelper = friendlyPublishedContentHelper;
        _layoutViewModelFactory = layoutViewModelFactory;
        _logger = logger;
    }

    public override IActionResult Index() {
        var startPage = CurrentPage as Core.Documents.StartPage;
        if(startPage == null) {
            return NotFound();
        }

        var pageNumber = HttpContext.GetCurrentPage();
        var pageSize = 10;

        var blogPosts = GetBlogPosts(startPage, pageNumber, pageSize, out var totalPosts);
        var viewModel = new StartPageViewModel(startPage, blogPosts) {
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalPosts = totalPosts
        };

        var layoutViewModel = _layoutViewModelFactory.Create(viewModel, startPage);
        return View(layoutViewModel);
    }

    private ICollection<Core.Documents.BlogPost> GetBlogPosts(Core.Documents.StartPage startPage, int pageNumber, int pageSize, out int totalPosts) {
        var blogIndex = _friendlyPublishedContentHelper
            .ChildrenOfType(startPage, BlogIndex.ModelTypeAlias)
            .FirstOrDefault();

        if(blogIndex == null) {
            _logger.LogWarning("No BlogIndex found for StartPage with ID {StartPageId}", startPage.Id);
            totalPosts = 0;
            return [];
        }

        var blogPosts = _friendlyPublishedContentHelper
            .ChildrenOfType(blogIndex, Core.Documents.BlogPost.ModelTypeAlias)
            .OfType<Core.Documents.BlogPost>()
            .OrderByDescending(x => x.PublishDate)
            .ToList();

        totalPosts = blogPosts.Count;

        return blogPosts.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
    }
}
