using Shorthand.UmbracoCodeFirst.Attributes;

namespace UmbracoBlog.Core.DataTypes;

[UCFDataType(
    "Mermaid Code Editor",
    typeof(UmbracoEditors.CodeEditor),
    typeof(UmbracoBlogMermaidCodeEditorConfiguration))]
[UCFFolderPath("CodeFirst")]
public partial class UmbracoBlogMermaidCodeEditorDataType;

internal sealed class UmbracoBlogMermaidCodeEditorConfiguration : UmbracoEditors.CodeEditor.Configuration {
    public UmbracoBlogMermaidCodeEditorConfiguration() {
        Height = 600;
        LineNumbers = true;
        Language = "mermaid";
    }
}
