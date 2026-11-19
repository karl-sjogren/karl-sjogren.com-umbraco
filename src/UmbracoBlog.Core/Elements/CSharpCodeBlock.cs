using Shorthand.UmbracoCodeFirst;
using Shorthand.UmbracoCodeFirst.Attributes;
using UmbracoBlog.Core.DataTypes;

namespace UmbracoBlog.Core.Elements;

[UCFElementType("CSharpCodeBlock", "C# Code block", AllowInLibrary = true, Icon = UmbracoIcons.Code)]
[UCFFolderPath("Elements")]
public partial class CSharpCodeBlock {
    [UCFProperty("Code",
        UmbracoBlogCSharpCodeEditorDataType.Alias,
        LabelOnTop = true)]
    public partial string Code { get; }
}
