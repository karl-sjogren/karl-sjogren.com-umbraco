using Shorthand.UmbracoCodeFirst.Attributes;
using UmbracoBlog.Core.Elements;

namespace UmbracoBlog.Core.DataTypes;

[UCFDataType(
    "Blog Post Content",
    typeof(UmbracoEditors.BlockList),
    typeof(UmbracoBlogPostContentConfiguration))]
[UCFFolderPath("CodeFirst")]
public partial class UmbracoBlogPostContentDataType;

internal sealed class UmbracoBlogPostContentConfiguration : UmbracoEditors.BlockList.Configuration {
    public UmbracoBlogPostContentConfiguration() {
        BlockTypes = [
            new(typeof(CodeBlock)) { Label = $"Code block: ${{{CodeBlock.PropertyAliases.Filename} || {CodeBlock.PropertyAliases.Language} }} - {{umbValue: {CodeBlock.PropertyAliases.Code} | truncate: 48}}", IconColor = "#c4b320", BackgroundColor = "#1b4e89" },
            new(typeof(CSharpCodeBlock)) { Label = "C# Code block", IconColor = "#e74918", BackgroundColor = "#1b4e89" },
            new(typeof(JavascriptCodeBlock)) { Label = "Javascript block", IconColor = "#e76317", BackgroundColor = "#1b4e89" },
            new(typeof(ShellCodeBlock)) { Label = "Shell block", IconColor = "#6adb1e", BackgroundColor = "#1b4e89" },
            new(ContentBlock.ModelTypeAlias) { Label = $"Content block: {{umbValue: {ContentBlock.PropertyAliases.Content} | stripHtml | truncate: 64}}", BackgroundColor = "#63ece6" },
            new(ImageBlock.ModelTypeAlias) { Label = $"Image block: {{umbContentName: {ImageBlock.PropertyAliases.Image} | truncate: 64}}", BackgroundColor = "#f0a500" },
            new(MermaidBlock.ModelTypeAlias) { Label = $"Mermaid block: {{umbValue: {MermaidBlock.PropertyAliases.Script} | truncate: 64}}", BackgroundColor = "#1b4e89" },
            new(NoticeBlock.ModelTypeAlias) { Label = $"Notice block: {{umbValue: {NoticeBlock.PropertyAliases.Type}}} - {{umbValue: {NoticeBlock.PropertyAliases.Text}}}", BackgroundColor = "#ff6f61" }
        ];
        UseInlineEditingAsDefault = true;
        CreateModalSize = "medium";
    }
}
