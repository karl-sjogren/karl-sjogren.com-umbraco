using Shorthand.UmbracoCodeFirst.Attributes;

namespace UmbracoBlog.Core.DataTypes;

[UCFDataType(
    "Rich Text",
    typeof(UmbracoEditors.RichTextEditor),
    typeof(UmbracoBlogRichTextConfiguration))]
[UCFFolderPath("CodeFirst")]
public partial class UmbracoBlogRichTextDataType;

internal sealed class UmbracoBlogRichTextConfiguration : UmbracoEditors.RichTextEditor.Configuration {
    public UmbracoBlogRichTextConfiguration() {
    }
}
