export const manifests: Array<UmbExtensionManifest> = [
  {
    type: "modal",
    alias: "UmbracoBlog.IconPickerModal",
    name: "Umbraco Blog Icon Picker Modal",
    js: () => import("./dialog")
  },
  {
    type: "propertyEditorUi",
    alias: "UmbracoBlog.IconPicker.PropertyEditor",
    name: "Umbraco Blog Icon Picker",
    js: () => import("./property-editor.js"),
    meta: {
      label: "Icon Picker",
      icon: "icon-picture",
      group: "pickers",
      propertyEditorSchemaAlias: "Umbraco.Plain.String",
      supportsReadOnly: true
    }
  }
];
