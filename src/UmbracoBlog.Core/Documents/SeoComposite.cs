using Shorthand.UmbracoCodeFirst;
using Shorthand.UmbracoCodeFirst.Attributes;
using UmbracoBlog.Core.DataTypes;

namespace UmbracoBlog.Core.Documents;

[UCFDocumentType(
    "SEO Composite",
    Icon = UmbracoIcons.Search)]
[UCFFolderPath("Compositions")]
public partial class SeoComposite {
    [UCFProperty(
        "SEO Title",
        UmbracoBlogTextDataType.Alias,
        SortOrder = 0,
        Mandatory = false)]
    [UCFDescription("The SEO title for the page.")]
    [SeoTab]
    public partial string SeoTitle { get; }

    [UCFProperty(
        "SEO Description",
        UmbracoBlogTextDataType.Alias,
        SortOrder = 1,
        Mandatory = false)]
    [UCFDescription("The SEO description for the page.")]
    [SeoTab]
    public partial string SeoDescription { get; }
}

[AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
internal sealed class SeoTabAttribute()
    : UCFTabAttribute("SEO", 0);
