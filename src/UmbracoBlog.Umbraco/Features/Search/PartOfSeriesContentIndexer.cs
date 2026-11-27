using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Search.Core.Extensions;
using Umbraco.Cms.Search.Core.Models.Indexing;
using Umbraco.Cms.Search.Core.Services.ContentIndexing;

namespace UmbracoBlog.Umbraco.Features.Search;

public class PartOfSeriesContentIndexer : IContentIndexer {
    public Task<IEnumerable<IndexField>> GetIndexFieldsAsync(
            IContentBase content,
            string?[] cultures,
            bool published,
            CancellationToken cancellationToken) {
        var propertyAlias = Core.Documents.BlogPost.PropertyAliases.PartOfSeries;
        if(!content.HasProperty(propertyAlias)) {
            return Task.FromResult<IEnumerable<IndexField>>([]);
        }

        var partOfSeriesJson = content.GetValue<string>(propertyAlias);
        if(string.IsNullOrWhiteSpace(partOfSeriesJson)) {
            return Task.FromResult<IEnumerable<IndexField>>([]);
        }

        var partOfSeriesArray = System.Text.Json.JsonSerializer.Deserialize<Guid[]>(partOfSeriesJson);
        if(partOfSeriesArray is null || partOfSeriesArray.Length == 0) {
            return Task.FromResult<IEnumerable<IndexField>>([]);
        }

        var partOfSeriesGuid = partOfSeriesArray.First();

        return Task.FromResult<IEnumerable<IndexField>>([
            new IndexField(
                "UmbracoBlog_PartOfSeries",
                new IndexValue { Keywords = [partOfSeriesGuid.AsKeyword()] },
                Culture: null,
                Segment: null
            )
        ]);
    }
}
