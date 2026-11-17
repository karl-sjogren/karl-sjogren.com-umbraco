using Shorthand.UmbracoCodeFirst;
using Shorthand.UmbracoCodeFirst.Attributes;
using UmbracoBlog.Core.DataTypes;

namespace UmbracoBlog.Core.Elements;

[UCFElementType("Shell Code block", AllowInLibrary = true, Icon = UmbracoIcons.Code)]
[UCFFolderPath("Elements")]
public partial class ShellCodeBlock {
    [UCFProperty("Code",
        UmbracoBlogShellCodeEditorDataType.Alias,
        LabelOnTop = true)]
    public partial string Code { get; }
}
