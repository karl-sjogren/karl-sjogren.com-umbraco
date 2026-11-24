using Shorthand.UmbracoCodeFirst.Attributes;

namespace UmbracoBlog.Core.DataTypes;

[UCFDataType(
    "Icon Picker",
    "Umbraco.Plain.String",
    "UmbracoBlog.IconPicker.PropertyEditor")]
[UCFFolderPath("CodeFirst")]
public partial class UmbracoBlogIconPickerDataType;
