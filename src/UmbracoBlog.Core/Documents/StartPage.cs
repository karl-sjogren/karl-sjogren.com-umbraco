using Shorthand.UmbracoCodeFirst;
using Shorthand.UmbracoCodeFirst.Attributes;
using UmbracoBlog.Core.DataTypes;

namespace UmbracoBlog.Core.Documents;

[UCFDocumentType(
    "Start Page",
    Icon = UmbracoIcons.Star)]
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

[UCFDocumentType(
    "Blog Entry",
    Icon = UmbracoIcons.Document)]
[UCFComposite(typeof(SeoComposite))]
public partial class BlogEntry {
    [UCFProperty(
        "Main heading",
        UmbracoBlogTextDataType.Alias,
        SortOrder = 0,
        Mandatory = true)]
    [UCFDescription("The main heading for the blog entry.")]
    [ContentTab]
    public partial string MainHeading { get; }

    [UCFProperty(
        "Main content",
        UmbracoBlogRichTextDataType.Alias,
        SortOrder = 1,
        Mandatory = false)]
    [UCFDescription("The main content for the blog entry.")]
    [ContentTab]
    public partial string MainContent { get; }
}
