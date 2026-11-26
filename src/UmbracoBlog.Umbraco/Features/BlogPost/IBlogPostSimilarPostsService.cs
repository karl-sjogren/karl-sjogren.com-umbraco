namespace UmbracoBlog.Umbraco.Features.BlogPost;

public interface IBlogPostSimilarPostsService {
    ICollection<Core.Documents.BlogPost> GetSimilarPosts(Core.Documents.BlogPost blogPost, int maxResults);
}
