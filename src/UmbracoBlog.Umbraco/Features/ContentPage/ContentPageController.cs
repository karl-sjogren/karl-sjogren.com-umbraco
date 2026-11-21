using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Web.Common.Controllers;
using UmbracoBlog.Core.Contracts;

namespace UmbracoBlog.Umbraco.Features.ContentPage;

public class ContentPageController : RenderController {
    private readonly ILayoutViewModelFactory _layoutViewModelFactory;
    private readonly ILogger<ContentPageController> _logger;

    public ContentPageController(
            ILayoutViewModelFactory layoutViewModelFactory,
            ICompositeViewEngine compositeViewEngine,
            IUmbracoContextAccessor umbracoContextAccessor,
            ILogger<ContentPageController> logger)
        : base(logger, compositeViewEngine, umbracoContextAccessor) {
        _layoutViewModelFactory = layoutViewModelFactory;
        _logger = logger;
    }

    public override IActionResult Index() {
        var contentPage = CurrentPage as Core.Documents.ContentPage;
        if(contentPage == null) {
            return NotFound();
        }

        var layoutViewModel = _layoutViewModelFactory.Create(contentPage, contentPage);
        return View(layoutViewModel);
    }
}
