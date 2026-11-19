using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Shorthand.UmbracoHelpers;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Web.Common.Controllers;
using UmbracoBlog.Core.Documents;
using UmbracoBlog.Core.ViewModels;

namespace UmbracoBlog.Core.Controllers;

public class StartPageController : RenderController {
    private readonly IFriendlyPublishedContentHelper _friendlyPublishedContentHelper;
    private readonly ILogger<StartPageController> _logger;

    public StartPageController(
            IFriendlyPublishedContentHelper friendlyPublishedContentHelper,
            ICompositeViewEngine compositeViewEngine,
            IUmbracoContextAccessor umbracoContextAccessor,
            ILogger<StartPageController> logger)
        : base(logger, compositeViewEngine, umbracoContextAccessor) {
        _friendlyPublishedContentHelper = friendlyPublishedContentHelper;
        _logger = logger;
    }

    public override IActionResult Index() {
        var startPage = CurrentPage as StartPage;
        if(startPage == null) {
            return NotFound();
        }

        var blogPosts = GetBlogPosts(startPage);
        var viewModel = new StartPageViewModel(startPage, blogPosts);

        return CurrentTemplate(viewModel);
    }

    private ICollection<BlogPost> GetBlogPosts(StartPage startPage) {
        var blogIndex = _friendlyPublishedContentHelper
            .ChildrenOfType(startPage, BlogIndex.ModelTypeAlias)
            .FirstOrDefault();

        if(blogIndex == null) {
            _logger.LogWarning("No BlogIndex found for StartPage with ID {StartPageId}", startPage.Id);
            return [];
        }

        var blogPosts = _friendlyPublishedContentHelper
            .ChildrenOfType(blogIndex, BlogPost.ModelTypeAlias)
            .OfType<BlogPost>()
            .ToList();

        return [.. blogPosts];
    }
}
