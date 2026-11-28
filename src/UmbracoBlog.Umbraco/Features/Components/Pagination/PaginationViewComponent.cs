using Microsoft.AspNetCore.Mvc;
using Shorthand.UmbracoHelpers;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace UmbracoBlog.Umbraco.Features.Components.Pagination;

public class PaginationViewComponent : ViewComponent {
    private readonly IFriendlyPublishedContentHelper _friendlyPublishedContentHelper;
    private readonly ILogger<PaginationViewComponent> _logger;

    public PaginationViewComponent(
            IFriendlyPublishedContentHelper friendlyPublishedContentHelper,
            ILogger<PaginationViewComponent> logger) {
        _friendlyPublishedContentHelper = friendlyPublishedContentHelper;
        _logger = logger;
    }

    public IViewComponentResult Invoke(int currentPage, int pageSize, int totalItems, IPublishedContent content) {
        var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);
        var pageNumberLinks = new List<PaginationLinkViewModel>();
        for(var i = 1; i <= totalPages; i++) {
            pageNumberLinks.Add(new PaginationLinkViewModel {
                Label = i.ToString(),
                CssClass = "pagination-page" + (i == currentPage ? " current" : ""),
                Url = GetPageUrl(content, i),
                IsCurrent = i == currentPage
            });
        }

        if(currentPage > 1) {
            pageNumberLinks.Insert(0, new PaginationLinkViewModel {
                Label = "Previous",
                CssClass = "pagination-previous",
                Url = GetPageUrl(content, currentPage - 1),
                IsCurrent = false
            });
        }

        if(currentPage < totalPages) {
            pageNumberLinks.Add(new PaginationLinkViewModel {
                Label = "Next",
                CssClass = "pagination-next",
                Url = GetPageUrl(content, currentPage + 1),
                IsCurrent = false
            });
        }

        var viewModel = new PaginationViewModel {
            PageNumberLinks = pageNumberLinks
        };

        return View(viewModel);
    }

    internal string GetPageUrl(IPublishedContent content, int pageNumber) {
        var baseUrl = _friendlyPublishedContentHelper.Url(content);
        var paginatedUrl = baseUrl + "page-" + pageNumber;

        var queryString = ViewContext.HttpContext.Request.QueryString;
        return paginatedUrl + queryString;
    }
}
