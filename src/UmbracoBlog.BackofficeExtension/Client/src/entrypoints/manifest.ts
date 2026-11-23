export const manifests: Array<UmbExtensionManifest> = [
  {
    name: "Umbraco Blog Backoffice Extension Entrypoint",
    alias: "UmbracoBlog.BackofficeExtension.Entrypoint",
    type: "backofficeEntryPoint",
    js: () => import("./entrypoint.js"),
  },
];
