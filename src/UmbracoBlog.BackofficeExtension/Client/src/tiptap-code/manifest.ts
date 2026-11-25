export const manifests: Array<UmbExtensionManifest> = [
  {
    type: "tiptapExtension",
    kind: "button",
    alias: "UmbracoBlog.Tiptap.Code",
    name: "Umbraco Blog Inline Code Tiptap Extension",
    api: () => import("./code.tiptap-api.js"),
    meta: {
      icon: "icon-code",
      label: "Inline code",
      group: "#tiptap_extGroup_formatting",
    },
  },
  {
    type: "tiptapToolbarExtension",
    kind: "button",
    alias: "UmbracoBlog.Tiptap.Toolbar.Code",
    name: "Umbraco Blog Inline Code Tiptap Toolbar Extension",
    api: () => import("./code.tiptap-toolbar-api.js"),
    forExtensions: ["UmbracoBlog.Tiptap.Code"],
    meta: {
      alias: "code",
      icon: "icon-code",
      label: "Inline code",
    },
  },
];
