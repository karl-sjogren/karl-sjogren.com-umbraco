using UmbracoBlog.Core.ViewModels;

namespace UmbracoBlog.Umbraco.Features.TagsPage;

public class TagDetailsViewModel : ViewModelBase<Core.Documents.TagsPage> {
    public string Tag { get; }
    public ICollection<TagsDetailsBlogPostViewModel> BlogPosts { get; }

    public TagDetailsViewModel(Core.Documents.TagsPage content, string tag, ICollection<TagsDetailsBlogPostViewModel> blogPosts)
        : base(content) {
        Tag = tag;
        BlogPosts = blogPosts;
    }
}

public record TagsDetailsBlogPostViewModel(string Title, DateTime PublishDate, string Url);
