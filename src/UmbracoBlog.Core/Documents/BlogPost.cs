using Shorthand.UmbracoCodeFirst;
using Shorthand.UmbracoCodeFirst.Attributes;
using Umbraco.Cms.Core.Models.Blocks;
using Umbraco.Cms.Core.Models.PublishedContent;
using UmbracoBlog.Core.DataTypes;

namespace UmbracoBlog.Core.Documents;

[UCFDocument(
    "Blog Post",
    Icon = UmbracoIcons.Star,
    AllowAsRoot = false,
    DefaultTemplateAlias = UmbracoTemplates.BlogPost.Index,
    AllowedTemplateAliases = new[] { UmbracoTemplates.BlogPost.Index })]
[UCFComposite(typeof(Composites.SeoComposite))]
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
        "Publish date",
        UmbracoBlogPublishDateDataType.Alias,
        SortOrder = 1,
        Mandatory = true)]
    [UCFDescription("The editorial publish date for the blog post.")]
    [ContentTab]
    public partial DateTime PublishDate { get; }

    [UCFProperty(
        "Main content",
        UmbracoBlogPostContentDataType.Alias,
        SortOrder = 2)]
    [UCFDescription("The main content for the blog post.")]
    [ContentTab]
    public partial BlockListModel MainContent { get; }

    [UCFProperty(
        "Part of series",
        UmbracoBlogBlogPostSeriesPickerDataType.Alias,
        SortOrder = 3,
        Mandatory = true)]
    [ContentTab]
    public partial IEnumerable<IPublishedElement> PartOfSeries { get; }

    [UCFProperty(
        "Tags",
        UmbracoBlogTagsDataType.Alias,
        SortOrder = 4)]
    [UCFDescription("The tags for the blog post.")]
    [ContentTab]
    public partial IEnumerable<string> Tags { get; }
}
