using Shorthand.UmbracoCodeFirst.Attributes;

namespace UmbracoBlog.Core.DataTypes;

[UCFDataType(
    "Javascript Code Editor",
    typeof(UmbracoEditors.CodeEditor),
    typeof(UmbracoBlogJavascriptCodeEditorConfiguration))]
[UCFFolderPath("CodeFirst")]
public partial class UmbracoBlogJavascriptCodeEditorDataType;

internal sealed class UmbracoBlogJavascriptCodeEditorConfiguration : UmbracoEditors.CodeEditor.Configuration {
    public UmbracoBlogJavascriptCodeEditorConfiguration() {
        Height = 600;
        LineNumbers = true;
        Language = "javascript";
    }
}
