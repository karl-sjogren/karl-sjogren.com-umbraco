using Shorthand.UmbracoHelpers;
using UmbracoBlog.Core.Documents.Composites;

namespace UmbracoBlog.Umbraco.Features.Components.Navigation;

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
