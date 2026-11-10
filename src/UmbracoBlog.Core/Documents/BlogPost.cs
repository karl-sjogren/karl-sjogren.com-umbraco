using Shorthand.UmbracoCodeFirst;
using Shorthand.UmbracoCodeFirst.Attributes;
using Umbraco.Cms.Core.Models.Blocks;
using UmbracoBlog.Core.DataTypes;

namespace UmbracoBlog.Core.Documents;

[UCFDocumentType(
    "Blog Post",
    Icon = UmbracoIcons.Star,
    AllowAsRoot = false)]
[UCFComposite(typeof(SeoComposite))]
public partial class BlogPost {
    [UCFProperty(
        "Main heading",
        UmbracoBlogTextDataType.Alias,
        SortOrder = 0,
        Mandatory = true)]
    [UCFDescription("The main heading for the blog post.")]
    [ContentTab]
    public partial string MainHeading { get; }

    [UCFProperty(
        "Main content",
        UmbracoBlogPostContentDataType.Alias,
        SortOrder = 1)]
    [UCFDescription("The main content for the blog post.")]
    [ContentTab]
    public partial IEnumerable<BlockListItem> MainContent { get; }

    [UCFProperty(
        "Tags",
        UmbracoBlogTagsDataType.Alias,
        SortOrder = 2)]
    [UCFDescription("The tags for the blog post.")]
    [ContentTab]
    public partial IEnumerable<string> Tags { get; }
}
