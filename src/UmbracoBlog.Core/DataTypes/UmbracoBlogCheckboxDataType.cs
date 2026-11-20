using Shorthand.UmbracoCodeFirst.Attributes;

namespace UmbracoBlog.Core.DataTypes;

[UCFDataType("Boolean",
    typeof(UmbracoEditors.Boolean),
    typeof(UmbracoBlogCodeEditorConfiguration))]
[UCFFolderPath("CodeFirst")]
public partial class UmbracoBlogBooleanDataType;

internal sealed class UmbracoBlogBooleanConfiguration : UmbracoEditors.Boolean.Configuration {
}
