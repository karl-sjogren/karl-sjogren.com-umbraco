using Shorthand.UmbracoCodeFirst;
using Shorthand.UmbracoCodeFirst.Attributes;
using UmbracoBlog.Core.DataTypes;

namespace UmbracoBlog.Core.Elements;

[UCFElementType("Code block", AllowInLibrary = true, Icon = UmbracoIcons.Code)]
public partial class CodeBlock {
    [UCFProperty("Language",
        UmbracoBlogCodingLanguageDataType.Alias)]
    public partial string Language { get; }

    [UCFProperty("Code",
        UmbracoBlogCodingLanguageDataType.Alias,
        LabelOnTop = true)]
    public partial string Code { get; }
}
