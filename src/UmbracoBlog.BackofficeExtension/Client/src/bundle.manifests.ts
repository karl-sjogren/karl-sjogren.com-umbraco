import { manifests as entrypoints } from "./entrypoints/manifest.js";
import { manifests as iconPicker } from "./icon-picker/manifest.ts";
import { manifests as tiptapCode } from "./tiptap-code/manifest.ts";

// Job of the bundle is to collate all the manifests from different parts of the extension and load other manifests
// We load this bundle from umbraco-package.json
export const manifests: Array<UmbExtensionManifest> = [
  ...entrypoints,
  ...iconPicker,
  ...tiptapCode,
];
