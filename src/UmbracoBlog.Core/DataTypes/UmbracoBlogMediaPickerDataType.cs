using Shorthand.UmbracoCodeFirst.Attributes;
using Shorthand.UmbracoCodeFirst.Metadata;

namespace UmbracoBlog.Core.DataTypes;

[UCFDataType(
    "Media Picker",
    typeof(UmbracoEditors.MediaPicker),
    typeof(UmbracoBlogMediaPickerDataType))]
[UCFFolderPath("CodeFirst")]
public partial class UmbracoBlogMediaPickerDataType;

public sealed partial class UmbracoBlogMediaPickerDataType : UmbracoEditors.MediaPicker.Configuration {
    public UmbracoBlogMediaPickerDataType() {
        AcceptedMediaTypeKeys = [UmbracoConstants.MediaTypes.Guids.ImageGuid];
        Multiple = false;
        EnableLocalFocalPoint = false;
        Crops = [new CodeFirstMediaPickerCrop("thumbnail", 600, 400)];
    }
}
