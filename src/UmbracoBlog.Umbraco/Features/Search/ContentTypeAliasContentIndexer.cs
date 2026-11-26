using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Search.Core.Models.Indexing;
using Umbraco.Cms.Search.Core.Services.ContentIndexing;

namespace UmbracoBlog.Umbraco.Features.Search;

public class ContentTypeAliasContentIndexer : IContentIndexer {
    public Task<IEnumerable<IndexField>> GetIndexFieldsAsync(
            IContentBase content,
            string?[] cultures,
            bool published,
            CancellationToken cancellationToken)
        => Task.FromResult<IEnumerable<IndexField>>([
            new IndexField(
                "UmbracoBlog_ContentTypeAlias",
                new IndexValue { Keywords = [content.ContentType.Alias] },
                Culture: null,
                Segment: null
            )
        ]);
}
