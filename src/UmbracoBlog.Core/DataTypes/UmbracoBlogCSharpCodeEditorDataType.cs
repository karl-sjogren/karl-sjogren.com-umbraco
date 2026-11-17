using Shorthand.UmbracoCodeFirst.Attributes;

namespace UmbracoBlog.Core.DataTypes;

[UCFDataType(
    "C# Code Editor",
    typeof(UmbracoEditors.CodeEditor),
    typeof(UmbracoBlogCSharpCodeEditorConfiguration))]
[UCFFolderPath("CodeFirst")]
public partial class UmbracoBlogCSharpCodeEditorDataType;

internal sealed class UmbracoBlogCSharpCodeEditorConfiguration : UmbracoEditors.CodeEditor.Configuration {
    public UmbracoBlogCSharpCodeEditorConfiguration() {
        Height = 600;
        LineNumbers = true;
        Language = "csharp";
    }
}
