using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Shorthand.UmbracoHelpers;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Web.Common.Controllers;
using UmbracoBlog.Core.Contracts;

namespace UmbracoBlog.Umbraco.Features.BlogPost;

public class BlogPostController : RenderController {
    private readonly IBlogPostSimilarPostsService _blogPostSimilarPostsService;
    private readonly IFriendlyPublishedContentHelper _friendlyPublishedContentHelper;
    private readonly ILayoutViewModelFactory _layoutViewModelFactory;
    private readonly ILogger<BlogPostController> _logger;

    public BlogPostController(
            IBlogPostSimilarPostsService blogPostSimilarPostsService,
            IFriendlyPublishedContentHelper friendlyPublishedContentHelper,
            ILayoutViewModelFactory layoutViewModelFactory,
            ICompositeViewEngine compositeViewEngine,
            IUmbracoContextAccessor umbracoContextAccessor,
            ILogger<BlogPostController> logger)
        : base(logger, compositeViewEngine, umbracoContextAccessor) {
        _blogPostSimilarPostsService = blogPostSimilarPostsService;
        _friendlyPublishedContentHelper = friendlyPublishedContentHelper;
        _layoutViewModelFactory = layoutViewModelFactory;
        _logger = logger;
    }

    public override IActionResult Index() {
        var blogPost = CurrentPage as Core.Documents.BlogPost;
        if(blogPost == null) {
            return NotFound();
        }

        var similarPosts = _blogPostSimilarPostsService.GetSimilarPosts(blogPost, 6);

        var similarViewModels = similarPosts.Select(p => new BlogPostSimilarPostViewModel(p.Name, p.PublishDate, _friendlyPublishedContentHelper.Url(p))).ToArray();

        var viewModel = new BlogPostViewModel(blogPost, similarViewModels);

        var layoutViewModel = _layoutViewModelFactory.Create(viewModel, blogPost);
        return View(layoutViewModel);
    }
}
