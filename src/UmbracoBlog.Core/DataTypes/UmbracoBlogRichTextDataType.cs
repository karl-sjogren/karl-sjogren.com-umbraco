using Shorthand.UmbracoCodeFirst.Attributes;

namespace UmbracoBlog.Core.DataTypes;

[UCFDataType(
    "Rich Text",
    typeof(UmbracoEditors.RichTextEditor),
    typeof(UmbracoBlogRichTextConfiguration))]
[UCFFolderPath("CodeFirst")]
public partial class UmbracoBlogRichTextDataType;

internal sealed class UmbracoBlogRichTextConfiguration : UmbracoEditors.RichTextEditor.Configuration {
    public UmbracoBlogRichTextConfiguration() {
        ToolbarRows = [
            [
                [TiptapToolbar.SourceEditor, TiptapToolbar.ClearFormatting],
                [TiptapToolbar.Heading2, TiptapToolbar.Heading3, TiptapToolbar.Heading4],
                [TiptapToolbar.Bold, TiptapToolbar.Italic, TiptapToolbar.Underline, _inlineCodeToolbar],
                [TiptapToolbar.TextAlignLeft, TiptapToolbar.TextAlignCenter, TiptapToolbar.TextAlignRight],
                [TiptapToolbar.BulletList, TiptapToolbar.OrderedList],
                [TiptapToolbar.Blockquote, TiptapToolbar.HorizontalRule],
                [TiptapToolbar.Link, TiptapToolbar.Unlink],
            ]
        ];

        StatusbarGroups = [
            [TiptapStatusbar.WordCount]
        ];

        Extensions!.Add(TiptapExtension.WordCount);
        Extensions.Add(_inlineCodeExtension);
    }

    // Registered by UmbracoBlog.BackofficeExtension (Client/src/tiptap-code)
    private const string _inlineCodeExtension = "UmbracoBlog.Tiptap.Code";
    private const string _inlineCodeToolbar = "UmbracoBlog.Tiptap.Toolbar.Code";
}
