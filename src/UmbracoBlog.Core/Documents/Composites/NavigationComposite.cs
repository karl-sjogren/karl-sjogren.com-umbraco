using Shorthand.UmbracoCodeFirst;
using Shorthand.UmbracoCodeFirst.Attributes;
using UmbracoBlog.Core.DataTypes;

namespace UmbracoBlog.Core.Documents.Composites;

[UCFDocument(
    "Navigation Composite",
    Icon = UmbracoIcons.Search)]
[UCFFolderPath("Compositions")]
public partial class NavigationComposite {
    [UCFProperty(
        "Show in navigation",
        UmbracoBlogBooleanDataType.Alias,
        SortOrder = 0)]
    [UCFDescription("Whether to show the page in the navigation.")]
    [NavigationTab]
    public partial bool ShowInNavigation { get; }

    [UCFProperty(
        "Navigation icon",
        UmbracoBlogIconPickerDataType.Alias,
        SortOrder = 0,
        Mandatory = false)]
    [UCFDescription("The navigation icon for the page.")]
    [NavigationTab]
    public partial string NavigationIcon { get; }

    [UCFProperty(
        "Navigation name",
        UmbracoBlogTextDataType.Alias,
        SortOrder = 1)]
    [UCFDescription("The navigation name for the page.")]
    [NavigationTab]
    public partial string NavigationName { get; }
}

[AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
internal sealed class NavigationTabAttribute()
    : UCFTabAttribute("Navigation", 0);
