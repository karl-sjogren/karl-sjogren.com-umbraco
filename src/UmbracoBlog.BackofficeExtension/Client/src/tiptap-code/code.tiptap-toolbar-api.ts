import { UmbTiptapToolbarElementApiBase } from "@umbraco-cms/backoffice/tiptap";

type Editor = Parameters<UmbTiptapToolbarElementApiBase["execute"]>[0];

export default class UmbracoBlogTiptapToolbarCodeExtensionApi extends UmbTiptapToolbarElementApiBase {
  override execute(editor?: Editor) {
    editor?.chain().focus().toggleCode().run();
  }
}
