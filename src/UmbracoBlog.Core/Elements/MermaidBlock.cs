using Shorthand.UmbracoCodeFirst;
using Shorthand.UmbracoCodeFirst.Attributes;
using UmbracoBlog.Core.DataTypes;

namespace UmbracoBlog.Core.Elements;

[UCFElementType("Mermaid block", AllowInLibrary = true, Icon = UmbracoIcons.Code)]
[UCFFolderPath("Elements")]
public partial class MermaidBlock {
    [UCFProperty("Code",
        UmbracoBlogMermaidCodeEditorDataType.Alias,
        LabelOnTop = true)]
    public partial string Script { get; }
}
