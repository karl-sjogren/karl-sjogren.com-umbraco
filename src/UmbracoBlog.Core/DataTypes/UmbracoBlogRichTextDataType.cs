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
                [TiptapToolbar.Bold, TiptapToolbar.Italic, TiptapToolbar.Underline],
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
    }
}
