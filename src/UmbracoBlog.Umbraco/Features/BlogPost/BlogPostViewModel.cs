using UmbracoBlog.Core.ViewModels;

namespace UmbracoBlog.Umbraco.Features.BlogPost;

public class BlogPostViewModel : ViewModelBase<Core.Documents.BlogPost> {
    public ICollection<BlogPostSimilarPostViewModel> SimilarPosts { get; }

    public BlogPostViewModel(Core.Documents.BlogPost content, ICollection<BlogPostSimilarPostViewModel> similarViewModels) : base(content) {
        SimilarPosts = similarViewModels;
    }
}

public record BlogPostSimilarPostViewModel(string Title, DateTime PublishDate, string Url);
