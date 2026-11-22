using Shorthand.UmbracoCodeFirst;
using Shorthand.UmbracoCodeFirst.Attributes;
using Umbraco.Cms.Core.Models.PublishedContent;
using UmbracoBlog.Core.DataTypes;

namespace UmbracoBlog.Core.Documents;

[UCFDocument(
    "Start Page",
    Icon = UmbracoIcons.Star,
    AllowAsRoot = true,
    AllowedChildAliases = new[] { BlogIndex.ModelTypeAlias, ContentPage.ModelTypeAlias, BlogArchive.ModelTypeAlias },
    DefaultTemplateAlias = UmbracoTemplates.StartPage.Index,
    AllowedTemplateAliases = new[] { UmbracoTemplates.StartPage.Index })]
[UCFComposite(typeof(Composites.SeoComposite))]
[UCFComposite(typeof(Composites.NavigationComposite))]
public partial class StartPage {
    [UCFProperty(
        "Main heading",
        UmbracoBlogTextDataType.Alias,
        SortOrder = 0,
        Mandatory = true)]
    [UCFDescription("The main heading for the start page.")]
    [ContentTab]
    public partial string MainHeading { get; }

    [UCFProperty(
        "Site settings",
        UmbracoBlogSiteSettingsPickerDataType.Alias,
        SortOrder = 1,
        Mandatory = true)]
    [ContentTab]
    public partial IEnumerable<IPublishedElement> SiteSettings { get; }
}
