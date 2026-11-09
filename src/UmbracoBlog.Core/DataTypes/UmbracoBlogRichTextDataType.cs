using Shorthand.UmbracoCodeFirst.Attributes;

namespace UmbracoBlog.Core.DataTypes;

[UCFDataType(
    "Rich Text",
    typeof(UmbracoEditors.RichTextEditor),
    typeof(UmbracoBlogRichTextDataType))]
[UCFFolderPath("CodeFirst")]
public partial class UmbracoBlogRichTextDataType;

internal sealed class UmbracoBlogRichTextConfiguration : UmbracoEditors.TextBox.Configuration {
    public UmbracoBlogRichTextConfiguration() {
        MaxChars = 255;
    }
}
