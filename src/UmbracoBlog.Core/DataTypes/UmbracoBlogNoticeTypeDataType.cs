using System.Text.Json;
using Shorthand.UmbracoCodeFirst.Attributes;

namespace UmbracoBlog.Core.DataTypes;

[UCFDataType(
    "Notice type",
    typeof(UmbracoEditors.Dropdown),
    typeof(UmbracoBlogNoticeTypeConfiguration))]
[UCFFolderPath("CodeFirst")]
public partial class UmbracoBlogNoticeTypeDataType;

internal sealed class UmbracoBlogNoticeTypeConfiguration : UmbracoEditors.Dropdown.Configuration {
    public UmbracoBlogNoticeTypeConfiguration() {
        Items = JsonSerializer.SerializeToElement(new[] {
            "info",
            "warning",
            "error",
            "success"
        });
    }
}
