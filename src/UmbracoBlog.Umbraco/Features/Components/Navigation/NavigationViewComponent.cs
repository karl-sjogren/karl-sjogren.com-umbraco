using Microsoft.AspNetCore.Mvc;
using Shorthand.UmbracoHelpers;
using Umbraco.Cms.Core;
using UmbracoBlog.Core.Documents.Composites;

namespace UmbracoBlog.Umbraco.Features.Components.Navigation;

public class NavigationViewComponent : ViewComponent {
    private readonly IFriendlyPublishedContentHelper _friendlyPublishedContentHelper;
    private readonly IPublishedContentQuery _publishedContentQuery;
    private readonly ILogger<NavigationViewComponent> _logger;

    public NavigationViewComponent(
            IFriendlyPublishedContentHelper friendlyPublishedContentHelper,
            IPublishedContentQuery publishedContentQuery,
            ILogger<NavigationViewComponent> logger) {
        _friendlyPublishedContentHelper = friendlyPublishedContentHelper;
        _publishedContentQuery = publishedContentQuery;
        _logger = logger;
    }

    public IViewComponentResult Invoke() {
        var startPage = GetStartPage();
        if(startPage is null) {
            return View(new NavigationViewModel {
                Items = []
            });
        }

        var children = _friendlyPublishedContentHelper
            .Children(startPage)
            .OfType<INavigationComposite>()
            .Where(x => x.ShowInNavigation)
            .Select(x => new NavigationItem(x, _friendlyPublishedContentHelper))
            .ToList();

        var startPageNavigationItem = new NavigationItem(startPage, _friendlyPublishedContentHelper);
        var viewModel = new NavigationViewModel {
            Items = [startPageNavigationItem, .. children]
        };

        return View(viewModel);
    }

    private Core.Documents.StartPage? GetStartPage() {
        var startPage = _publishedContentQuery.ContentAtRoot().OfType<Core.Documents.StartPage>().FirstOrDefault();
        if(startPage is null) {
            _logger.LogWarning("Start page not found at root");
        }

        return startPage;
    }
}

public record NavigationViewModel {
    public required ICollection<NavigationItem> Items { get; init; }
}

public record NavigationItem {
    public string Title { get; }
    public string Url { get; }
    public string? Icon { get; }

    public NavigationItem(INavigationComposite content, IFriendlyPublishedContentHelper friendlyPublishedContentHelper) {
        var name = content.NavigationName;
        if(string.IsNullOrEmpty(name)) {
            name = content.Name;
        }

        Title = name!;
        Url = friendlyPublishedContentHelper.Url(content);

        if(string.IsNullOrEmpty(content.NavigationIcon)) {
            Icon = null;
        } else {
            Icon = ParseIcon(content.NavigationIcon);
        }
    }

    internal static string ParseIcon(string? icon) {
        if(string.IsNullOrEmpty(icon)) {
            return "fas fa-circle";
        }

        var parts = icon.Split('|');
        if(parts.Length != 2) {
            return "fas fa-circle";
        }

        return parts[0] + " fa-" + parts[1];
    }
};
