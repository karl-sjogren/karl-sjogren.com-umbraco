using Shorthand.UmbracoCodeFirst;
using Shorthand.UmbracoCodeFirst.Attributes;
using UmbracoBlog.Core.DataTypes;

namespace UmbracoBlog.Core.Elements;

[UCFElementType("Blog post series", AllowInLibrary = true, Icon = UmbracoIcons.OrderedList)]
[UCFFolderPath("Elements")]
public partial class BlogPostSeriesElement {
    [UCFProperty("Series name",
        UmbracoBlogTextDataType.Alias,
        Mandatory = true)]
    public partial string SeriesName { get; }
}
