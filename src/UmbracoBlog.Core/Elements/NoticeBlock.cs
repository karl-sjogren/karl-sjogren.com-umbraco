using Shorthand.UmbracoCodeFirst;
using Shorthand.UmbracoCodeFirst.Attributes;
using UmbracoBlog.Core.DataTypes;

namespace UmbracoBlog.Core.Elements;

[UCFElementType("Notice block", AllowInLibrary = true, Icon = UmbracoIcons.Alert)]
[UCFFolderPath("Elements")]
public partial class NoticeBlock {
    [UCFProperty("Type",
        UmbracoBlogNoticeTypeDataType.Alias,
        LabelOnTop = true)]
    public partial string Type { get; }

    [UCFProperty("Text",
        UmbracoBlogTextDataType.Alias,
        LabelOnTop = true)]
    public partial string Text { get; }
}
