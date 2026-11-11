using Shorthand.UmbracoCodeFirst.Attributes;

namespace UmbracoBlog.Core.DataTypes;

[UCFDataType(
    "Code editor",
    typeof(UmbracoEditors.CodeEditor),
    typeof(UmbracoBlogCodeEditorConfiguration))]
[UCFFolderPath("CodeFirst")]
public partial class UmbracoBlogCodeEditorDataType;

internal sealed class UmbracoBlogCodeEditorConfiguration : UmbracoEditors.CodeEditor.Configuration {
    public UmbracoBlogCodeEditorConfiguration() {
        LineNumbers = true;
    }
}
