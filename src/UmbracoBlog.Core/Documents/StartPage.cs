using Shorthand.UmbracoCodeFirst;
using Shorthand.UmbracoCodeFirst.Attributes;
using UmbracoBlog.Core.DataTypes;

namespace UmbracoBlog.Core.Documents;

[UCFDocument(
    "Start Page",
    Icon = UmbracoIcons.Star,
    AllowAsRoot = true,
    AllowedChildAliases = new[] { BlogIndex.ModelTypeAlias },
    DefaultTemplateAlias = UmbracoTemplates.StartPage,
    AllowedTemplateAliases = new[] { UmbracoTemplates.StartPage })]
[UCFComposite(typeof(SeoComposite))]
public partial class StartPage {
    [UCFProperty(
        "Main heading",
        UmbracoBlogTextDataType.Alias,
        SortOrder = 0,
        Mandatory = true)]
    [UCFDescription("The main heading for the start page.")]
    [ContentTab]
    public partial string MainHeading { get; }
}
