using Shorthand.UmbracoCodeFirst;
using Shorthand.UmbracoCodeFirst.Attributes;

namespace UmbracoBlog.Core.Documents;

[UCFDocument(
    "Tags Page",
    Icon = UmbracoIcons.Tags,
    AllowAsRoot = false,
    DefaultTemplateAlias = UmbracoTemplates.TagsPage.Index,
    AllowedTemplateAliases = new[] { UmbracoTemplates.TagsPage.Index })]
[UCFComposite(typeof(Composites.SeoComposite))]
[UCFComposite(typeof(Composites.NavigationComposite))]
public partial class TagsPage {
}
