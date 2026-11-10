using Shorthand.UmbracoCodeFirst;
using Shorthand.UmbracoCodeFirst.Attributes;
using UmbracoBlog.Core.DataTypes;

namespace UmbracoBlog.Core.Elements;

[UCFElementType("Content block", AllowInLibrary = true, Icon = UmbracoIcons.Document)]
public partial class ContentBlock {
    [UCFProperty("Content",
        UmbracoBlogRichTextDataType.Alias,
        LabelOnTop = true)]
    public partial string Content { get; }
}
