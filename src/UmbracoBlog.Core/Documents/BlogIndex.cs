using Shorthand.UmbracoCodeFirst;
using Shorthand.UmbracoCodeFirst.Attributes;
using Shorthand.UmbracoCodeFirst.Metadata;

namespace UmbracoBlog.Core.Documents;

[UCFDocument(
    "Blog Index",
    Icon = UmbracoIcons.List,
    AllowedChildAliases = new[] { BlogPost.ModelTypeAlias })]
[UCFCollection(typeof(BlogIndexCollectionConfiguration))]
public partial class BlogIndex;

internal sealed class BlogIndexCollectionConfiguration : CodeFirstCollectionConfiguration<BlogPost> {
    public BlogIndexCollectionConfiguration() {
        Grid = false;
        PageSize = 25;
        OrderBy = Alias(post => post.PublishDate);
        OrderDirection = "desc";
        Columns = [
            new(Alias(post => post.MainHeading), "Title"),
            new(Alias(post => post.PublishDate), "Publish date"),
        ];
    }
}
