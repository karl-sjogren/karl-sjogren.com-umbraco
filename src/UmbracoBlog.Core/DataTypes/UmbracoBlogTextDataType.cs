using Shorthand.UmbracoCodeFirst.Attributes;

namespace UmbracoBlog.Core.DataTypes;

[UCFDataType(
    "Text",
    typeof(UmbracoEditors.TextBox),
    typeof(UmbracoBlogTextConfiguration))]
[UCFFolderPath("CodeFirst")]
public partial class UmbracoBlogTextDataType;

internal sealed class UmbracoBlogTextConfiguration : UmbracoEditors.TextBox.Configuration {
    public UmbracoBlogTextConfiguration() {
        MaxChars = 255;
    }
}
