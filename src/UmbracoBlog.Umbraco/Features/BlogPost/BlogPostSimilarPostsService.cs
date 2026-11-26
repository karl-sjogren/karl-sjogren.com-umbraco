using Umbraco.Cms.Core;
using Umbraco.Cms.Search.Core.Extensions;
using Umbraco.Cms.Search.Provider.Examine.Helpers;
using UmbracoBlog.Umbraco.Features.Search;

namespace UmbracoBlog.Umbraco.Features.BlogPost;

public class BlogPostSimilarPostsService : IBlogPostSimilarPostsService {
    private readonly IUmbracoBlogSearcher _searcher;
    private readonly IPublishedContentQuery _publishedContentQuery;
    private readonly ILogger<BlogPostSimilarPostsService> _logger;

    public BlogPostSimilarPostsService(
            IUmbracoBlogSearcher searcher,
            IPublishedContentQuery publishedContentQuery,
            ILogger<BlogPostSimilarPostsService> logger) {
        _searcher = searcher;
        _publishedContentQuery = publishedContentQuery;
        _logger = logger;
    }

    public ICollection<Core.Documents.BlogPost> GetSimilarPosts(Core.Documents.BlogPost blogPost, int maxResults) {
        var tags = blogPost.Tags?.ToArray() ?? [];

        if(tags.Length == 0) {
            return Array.Empty<Core.Documents.BlogPost>();
        }

        var results = _searcher.SearchWithMinimumShouldMatch(
            UmbracoSearchConstants.IndexAliases.PublishedContent,
            accessContext: null,
            field: Core.Documents.BlogPost.PropertyAliases.Tags,
            values: tags,
            minimumNumberShouldMatch: 1,
            skip: 0,
            take: maxResults,
            configureQuery: query => {
                return query
                    .And()
                    .Field(
                        FieldNameHelper.FieldName("UmbracoBlog_ContentTypeAlias", "keywords"),
                        Core.Documents.BlogPost.ModelTypeAlias)
                    .Not()
                    .Field(
                        FieldNameHelper.FieldName(UmbracoSearchConstants.FieldNames.Id, "keywords"),
                        blogPost.Key.AsKeyword());
            }
        );

        return results.Documents
            .Select(doc => _publishedContentQuery.Content(doc.Id))
            .OfType<Core.Documents.BlogPost>()
            .ToArray();
    }
}
