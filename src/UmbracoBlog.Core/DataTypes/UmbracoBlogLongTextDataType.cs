using Shorthand.UmbracoCodeFirst.Attributes;

namespace UmbracoBlog.Core.DataTypes;

[UCFDataType(
    "Long Text",
    typeof(UmbracoEditors.TextArea),
    typeof(UmbracoBlogLongTextConfiguration))]
[UCFFolderPath("CodeFirst")]
public partial class UmbracoBlogLongTextDataType;

internal sealed class UmbracoBlogLongTextConfiguration : UmbracoEditors.TextArea.Configuration {
    public UmbracoBlogLongTextConfiguration() {
        MaxChars = 5000;
        Rows = 8;
    }
}
