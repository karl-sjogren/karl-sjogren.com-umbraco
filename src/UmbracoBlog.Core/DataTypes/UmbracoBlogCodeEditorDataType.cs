using Shorthand.UmbracoCodeFirst.Attributes;

namespace UmbracoBlog.Core.DataTypes;

[UCFDataType(
    "Code Editor",
    typeof(UmbracoEditors.CodeEditor),
    typeof(UmbracoBlogCodeEditorConfiguration))]
[UCFFolderPath("CodeFirst")]
public partial class UmbracoBlogCodeEditorDataType;

internal sealed class UmbracoBlogCodeEditorConfiguration : UmbracoEditors.CodeEditor.Configuration {
    public UmbracoBlogCodeEditorConfiguration() {
        Height = 600;
        LineNumbers = true;
    }
}
