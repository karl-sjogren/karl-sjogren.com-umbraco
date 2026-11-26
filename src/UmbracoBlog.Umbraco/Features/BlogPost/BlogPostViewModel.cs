namespace UmbracoBlog.Umbraco.Features.BlogPost;

public class BlogPostViewModel {
    public required Core.Documents.BlogPost BlogPost { get; set; }
    public required ICollection<Core.Documents.BlogPost> SimilarPosts { get; set; }
}
