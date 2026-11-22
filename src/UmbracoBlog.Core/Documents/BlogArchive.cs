using Shorthand.UmbracoCodeFirst;
using Shorthand.UmbracoCodeFirst.Attributes;
using UmbracoBlog.Core.DataTypes;

namespace UmbracoBlog.Core.Documents;

[UCFDocument(
    "Blog Archive",
    Icon = UmbracoIcons.History,
    AllowAsRoot = false,
    DefaultTemplateAlias = UmbracoTemplates.BlogArchive.Index,
    AllowedTemplateAliases = new[] { UmbracoTemplates.BlogArchive.Index })]
[UCFComposite(typeof(Composites.SeoComposite))]
[UCFComposite(typeof(Composites.NavigationComposite))]
public partial class BlogArchive {
    [UCFProperty(
        "Main heading",
        UmbracoBlogTextDataType.Alias,
        SortOrder = 0,
        Mandatory = true)]
    [UCFDescription("The main heading for the blog archive.")]
    [ContentTab]
    public partial string MainHeading { get; }
}
