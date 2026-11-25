import { Code, UmbTiptapExtensionApiBase } from "@umbraco-cms/backoffice/tiptap";

export default class UmbracoBlogTiptapCodeExtensionApi extends UmbTiptapExtensionApiBase {
  getTiptapExtensions = () => [Code];
}
