using UmbracoBlog.Core.ViewModels;

namespace UmbracoBlog.Umbraco.Features.BlogArchive;

public class BlogArchiveViewModel : ViewModelBase<Core.Documents.BlogArchive> {
    public ICollection<BlogArchiveYearViewModel> BlogPosts { get; set; }

    public BlogArchiveViewModel(Core.Documents.BlogArchive content, ICollection<BlogArchiveYearViewModel> groupedBlogPosts)
            : base(content) {
        BlogPosts = groupedBlogPosts;
    }
}

public class BlogArchiveYearViewModel {
    public int Year { get; set; }
    public ICollection<BlogArchivePostViewModel> BlogPosts { get; set; }

    public BlogArchiveYearViewModel(int year, ICollection<BlogArchivePostViewModel> blogPosts) {
        Year = year;
        BlogPosts = blogPosts;
    }
}

public record BlogArchivePostViewModel(string Title, DateTime Date, string Url);
