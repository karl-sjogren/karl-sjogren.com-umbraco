using Umbraco.Cms.Core.Composing;

namespace UmbracoBlog.Umbraco.Features.BlogPost;

public class BlogPostComposer : IComposer {
    public void Compose(IUmbracoBuilder builder) {
        builder.Services.AddScoped<IBlogPostSimilarPostsService, BlogPostSimilarPostsService>();
    }
}
