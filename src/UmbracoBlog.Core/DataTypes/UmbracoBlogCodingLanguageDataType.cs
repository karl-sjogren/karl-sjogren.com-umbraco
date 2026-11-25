using System.Text.Json;
using Shorthand.UmbracoCodeFirst.Attributes;

namespace UmbracoBlog.Core.DataTypes;

[UCFDataType(
    "Coding language",
    typeof(UmbracoEditors.Dropdown),
    typeof(UmbracoBlogCodingLanguageConfiguration))]
[UCFFolderPath("CodeFirst")]
public partial class UmbracoBlogCodingLanguageDataType;

internal sealed class UmbracoBlogCodingLanguageConfiguration : UmbracoEditors.Dropdown.Configuration {
    public UmbracoBlogCodingLanguageConfiguration() {
        Items = JsonSerializer.SerializeToElement(new[] {
            "bash",
            "csharp",
            "html",
            "javascript",
            "json",
            "plaintext",
            "pwsh",
            "razor",
            "sass",
            "typescript",
            "xml"
        });
    }
}
