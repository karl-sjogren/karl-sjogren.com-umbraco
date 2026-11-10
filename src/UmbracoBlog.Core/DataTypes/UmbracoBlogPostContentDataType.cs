using System.Text.Json;
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
        this.Blocks = JsonSerializer.SerializeToElement(new[] {
            CodeBlock.ModelTypeAlias,
            ContentBlock.ModelTypeAlias
        });
    }
}
