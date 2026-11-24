using Shorthand.UmbracoCodeFirst;
using Shorthand.UmbracoCodeFirst.Attributes;

namespace UmbracoBlog.Core.Documents;

[UCFDocument(
    "Search Page",
    Icon = UmbracoIcons.Search,
    AllowAsRoot = false,
    DefaultTemplateAlias = UmbracoTemplates.SearchPage.Index,
    AllowedTemplateAliases = new[] { UmbracoTemplates.SearchPage.Index })]
[UCFComposite(typeof(Composites.SeoComposite))]
[UCFComposite(typeof(Composites.NavigationComposite))]
public partial class SearchPage {
}
