using Shorthand.UmbracoCodeFirst.Attributes;

namespace UmbracoBlog.Core.DataTypes;

[UCFDataType(
    "Email",
    typeof(UmbracoEditors.EmailAddress),
    typeof(UmbracoBlogEmailConfiguration))]
[UCFFolderPath("CodeFirst")]
public partial class UmbracoBlogEmailDataType;

internal sealed class UmbracoBlogEmailConfiguration : UmbracoEditors.EmailAddress.Configuration {
    public UmbracoBlogEmailConfiguration() {
        InputType = "email";
    }
}
