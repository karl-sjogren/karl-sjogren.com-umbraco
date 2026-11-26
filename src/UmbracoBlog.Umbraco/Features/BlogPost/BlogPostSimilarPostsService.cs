using Examine;
using Examine.Lucene.Providers;
using Lucene.Net.Queries.Mlt;
using Lucene.Net.Search;
using Umbraco.Cms.Core;

namespace UmbracoBlog.Umbraco.Features.BlogPost;

public class BlogPostSimilarPostsService : IBlogPostSimilarPostsService {
    private readonly IExamineManager _examineManager;
    private readonly IPublishedContentQuery _publishedContentQuery;
    private readonly ILogger<BlogPostSimilarPostsService> _logger;

    public BlogPostSimilarPostsService(
            IExamineManager examineManager,
            IPublishedContentQuery publishedContentQuery,
            ILogger<BlogPostSimilarPostsService> logger) {
        _examineManager = examineManager;
        _publishedContentQuery = publishedContentQuery;
        _logger = logger;
    }

    public ICollection<Core.Documents.BlogPost> GetSimilarPosts(Core.Documents.BlogPost blogPost, int maxResults) {
        if(!_examineManager.TryGetIndex(UmbracoConstants.UmbracoIndexes.ExternalIndexName, out var index)) {
            throw new InvalidOperationException("Failed to get Examine index.");
        }

        var searcher = (LuceneSearcher)index.Searcher;
        var searchContext = searcher.GetSearchContext();

        using(var searchRef = searchContext.GetSearcher()) {
            var luceneIndexSearcher = searchRef.IndexSearcher;
            var moreLikeThis = new MoreLikeThis(luceneIndexSearcher.IndexReader) {
                FieldNames = ["nodeName", "mainHeading", "tags"],
                Analyzer = searcher.LuceneAnalyzer
            };

            var query = moreLikeThis.Like(GetDocId(blogPost));

            query = new BooleanQuery {
                { query, Occur.MUST },
                { new TermQuery(new Lucene.Net.Index.Term("__NodeTypeAlias", Core.Documents.BlogPost.ModelTypeAlias)), Occur.MUST }
            };

            var topDocs = luceneIndexSearcher.Search(query, maxResults);
            var similarPosts = topDocs.ScoreDocs
                .Select(sd => _publishedContentQuery.Content(sd.Doc))
                .OfType<Core.Documents.BlogPost>()
                .ToArray();

            return similarPosts;
        }
    }

    private int GetDocId(Core.Documents.BlogPost blogPost) {
        var index = _examineManager.GetIndex(UmbracoConstants.UmbracoIndexes.ExternalIndexName);
        var searcher = (LuceneSearcher)index.Searcher;
        var searchContext = searcher.GetSearchContext();

        using var searchRef = searchContext.GetSearcher();

        var luceneIndexSearcher = searchRef.IndexSearcher;
        var termQuery = new TermQuery(new Lucene.Net.Index.Term("__NodeId", blogPost.Id.ToString()));
        var topDocs = luceneIndexSearcher.Search(termQuery, 1);

        if(topDocs.ScoreDocs.Length == 0) {
            throw new InvalidOperationException("Failed to find document for blog post.");
        }

        return topDocs.ScoreDocs[0].Doc;
    }
}
