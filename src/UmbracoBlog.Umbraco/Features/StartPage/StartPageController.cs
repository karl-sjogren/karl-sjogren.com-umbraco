using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Shorthand.UmbracoHelpers;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Web.Common.Controllers;
using UmbracoBlog.Core.Contracts;
using UmbracoBlog.Core.Documents;

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

        var blogPosts = GetBlogPosts(startPage);
        var viewModel = new StartPageViewModel(startPage, blogPosts);

        var layoutViewModel = _layoutViewModelFactory.Create(viewModel, startPage);
        return View(layoutViewModel);
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
            .OrderByDescending(x => x.PublishDate)
            .ToList();

        return [.. blogPosts];
    }
}
