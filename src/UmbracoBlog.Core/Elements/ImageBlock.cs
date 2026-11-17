using Shorthand.UmbracoCodeFirst;
using Shorthand.UmbracoCodeFirst.Attributes;
using Umbraco.Cms.Core.Models;
using UmbracoBlog.Core.DataTypes;

namespace UmbracoBlog.Core.Elements;

[UCFElementType("Image block", AllowInLibrary = true, Icon = UmbracoIcons.DocumentImage)]
[UCFFolderPath("Elements")]
public partial class ImageBlock {
    [UCFProperty("Image", DataTypeKey = UmbracoConstants.DataTypes.Guids.MediaPicker3SingleImage)]
    public partial MediaWithCrops? Image { get; }

    [UCFProperty("Alt text",
        UmbracoBlogTextDataType.Alias)]
    public partial string AltText { get; }
}
