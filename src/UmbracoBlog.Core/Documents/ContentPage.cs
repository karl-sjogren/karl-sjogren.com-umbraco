using Shorthand.UmbracoCodeFirst;
using Shorthand.UmbracoCodeFirst.Attributes;
using Umbraco.Cms.Core.Models.Blocks;
using UmbracoBlog.Core.DataTypes;

namespace UmbracoBlog.Core.Documents;

[UCFDocument(
    "Content Page",
    Icon = UmbracoIcons.Document,
    AllowAsRoot = false,
    DefaultTemplateAlias = UmbracoTemplates.ContentPage.Index,
    AllowedTemplateAliases = new[] { UmbracoTemplates.ContentPage.Index })]
[UCFComposite(typeof(Composites.SeoComposite))]
[UCFComposite(typeof(Composites.NavigationComposite))]
public partial class ContentPage {
    [UCFProperty(
        "Main heading",
        UmbracoBlogTextDataType.Alias,
        SortOrder = 0,
        Mandatory = true)]
    [UCFDescription("The main heading for the content page.")]
    [ContentTab]
    public partial string MainHeading { get; }

    [UCFProperty(
        "Main content",
        UmbracoBlogPostContentDataType.Alias,
        SortOrder = 1)]
    [UCFDescription("The main content for the content page.")]
    [ContentTab]
    public partial BlockListModel MainContent { get; }
}
