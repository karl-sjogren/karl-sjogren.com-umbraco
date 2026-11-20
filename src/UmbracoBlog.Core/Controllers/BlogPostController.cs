using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Web.Common.Controllers;
using UmbracoBlog.Core.Contracts;
using UmbracoBlog.Core.Documents;

namespace UmbracoBlog.Core.Controllers;

public class BlogPostController : RenderController {
    private readonly ILayoutViewModelFactory _layoutViewModelFactory;
    private readonly ILogger<BlogPostController> _logger;

    public BlogPostController(
            ILayoutViewModelFactory layoutViewModelFactory,
            ICompositeViewEngine compositeViewEngine,
            IUmbracoContextAccessor umbracoContextAccessor,
            ILogger<BlogPostController> logger)
        : base(logger, compositeViewEngine, umbracoContextAccessor) {
        _layoutViewModelFactory = layoutViewModelFactory;
        _logger = logger;
    }

    public override IActionResult Index() {
        var blogPost = CurrentPage as BlogPost;
        if(blogPost == null) {
            return NotFound();
        }

        var layoutViewModel = _layoutViewModelFactory.Create(blogPost, blogPost);
        return CurrentTemplate(layoutViewModel);
    }
}
