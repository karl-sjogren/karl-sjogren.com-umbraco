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
            new(typeof(CodeBlock)) { Label = "Code block", IconColor = "#c4b320", BackgroundColor = "#1b4e89" },
            new(typeof(CSharpCodeBlock)) { Label = "Code block", IconColor = "#e74918", BackgroundColor = "#1b4e89" },
            new(typeof(JavascriptCodeBlock)) { Label = "Javascript block", IconColor = "#e76317", BackgroundColor = "#1b4e89" },
            new(typeof(ShellCodeBlock)) { Label = "Shell block", IconColor = "#6adb1e", BackgroundColor = "#1b4e89" },
            new(ContentBlock.ModelTypeAlias) { Label = "Content block", BackgroundColor = "#63ece6" },
        ];
        UseInlineEditingAsDefault = true;
        CreateModalSize = "medium";
    }
}
