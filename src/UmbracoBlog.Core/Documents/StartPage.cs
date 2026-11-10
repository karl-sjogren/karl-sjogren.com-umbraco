using Shorthand.UmbracoCodeFirst;
using Shorthand.UmbracoCodeFirst.Attributes;
using UmbracoBlog.Core.DataTypes;

namespace UmbracoBlog.Core.Documents;

[UCFDocumentType(
    "Start Page",
    Icon = UmbracoIcons.Star,
    AllowAsRoot = true,
    IsListView = true,
    AllowedChildAliases = new[] { BlogPost.ModelTypeAlias })]
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
