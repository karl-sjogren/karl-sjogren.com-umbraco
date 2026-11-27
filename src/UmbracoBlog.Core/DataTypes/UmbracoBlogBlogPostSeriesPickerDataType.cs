using Shorthand.UmbracoCodeFirst.Attributes;
using UmbracoBlog.Core.Elements;

namespace UmbracoBlog.Core.DataTypes;

[UCFDataType(
    "Blog Post Series Picker",
    typeof(UmbracoEditors.ElementPicker),
    typeof(UmbracoBlogBlogPostSeriesPickerConfiguration))]
[UCFFolderPath("CodeFirst")]
public partial class UmbracoBlogBlogPostSeriesPickerDataType;

internal sealed class UmbracoBlogBlogPostSeriesPickerConfiguration : UmbracoEditors.ElementPicker.Configuration {
    public UmbracoBlogBlogPostSeriesPickerConfiguration() {
        MaxNumber = 1;
        Filter = [BlogPostSeriesElement.ModelTypeAlias];
    }
}
