using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using Examine;
using Examine.Lucene.Search;
using Examine.Search;
using Lucene.Net.Index;
using Lucene.Net.Search;
using Microsoft.Extensions.Options;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Search.Core.Extensions;
using Umbraco.Cms.Search.Core.Models.Searching;
using Umbraco.Cms.Search.Provider.Examine.Configuration;
using Umbraco.Cms.Search.Provider.Examine.Helpers;
using Umbraco.Cms.Search.Provider.Examine.Services;

using UmbracoSearchResult = Umbraco.Cms.Search.Core.Models.Searching.SearchResult;

namespace UmbracoBlog.Umbraco.Features.Search;

public class UmbracoBlogSearcher : Searcher, IUmbracoBlogSearcher {
    private readonly IActiveIndexManager _activeIndexManager;
    private readonly ILogger<UmbracoBlogSearcher> _logger;

    //private static readonly ConcurrentDictionary<string, bool> _initializedIndexes = new();

    [UnsafeAccessor(UnsafeAccessorKind.StaticField, Name = "InitializedIndexes")]
    private static extern ref ConcurrentDictionary<string, bool> InitializedIndexes(Searcher @this);

    public UmbracoBlogSearcher(
            IExamineManager examineManager,
            IOptions<SearcherOptions> searcherOptions,
            IActiveIndexManager activeIndexManager,
            ILogger<UmbracoBlogSearcher> logger)
            : base(examineManager, searcherOptions, activeIndexManager) {
        _activeIndexManager = activeIndexManager;
        _logger = logger;
    }

    public UmbracoSearchResult SearchWithMinimumShouldMatch(
            string indexAlias,
            AccessContext? accessContext,
            string field,
            string[] values,
            int minimumNumberShouldMatch,
            int skip,
            int take,
            Func<IBooleanOperation, IBooleanOperation>? configureQuery = null) {
        var physicalIndexName = _activeIndexManager.ResolveActiveIndexName(indexAlias);
        if(!ExamineManager.TryGetIndex(physicalIndexName, out var index)) {
            return new UmbracoSearchResult(0, [], []);
        }

        EnsureFieldAnalyzersLoaded(index);

        var query = CreateQuery(index, accessContext, field, values, minimumNumberShouldMatch);
        query = configureQuery?.Invoke(query) ?? query;

        var results = query.Execute(new QueryOptions(skip, take));

        var searchResultDocuments = take > 0 ? results.Select(MapToDocument).WhereNotNull().ToArray() : [];

        return new UmbracoSearchResult(results.TotalItemCount, searchResultDocuments, []);
    }

    private IBooleanOperation CreateQuery(IIndex index, AccessContext? accessContext, string field, string[] values, int minimumNumberShouldMatch) {
        var luceneQuery = (LuceneSearchQuery)index.Searcher
            .CreateQuery();

        var keywordField = FieldNameHelper.FieldName(field, "keywords");
        var terms = values.Select(value => new Term(keywordField, value)).ToArray();

        var minMatchBooleanQuery = new BooleanQuery() {
            MinimumNumberShouldMatch = minimumNumberShouldMatch
        };

        foreach(var term in terms) {
            minMatchBooleanQuery.Add(new TermQuery(term), Occur.SHOULD);
        }

        var baseQuery = luceneQuery.LuceneQuery(minMatchBooleanQuery, BooleanOperation.And);

        AddProtection(baseQuery, accessContext);
        return baseQuery;
    }

    private void AddProtection(IBooleanOperation searchQuery, AccessContext? accessContext) {
        if(accessContext?.Bypass is true) {
            return;
        }

        if(accessContext is null || accessContext.PrincipalId == Guid.Empty) {
            searchQuery.And().Field(SystemFields.Protection, Guid.Empty.AsKeyword());
        } else {
            List<string> keys = [Guid.Empty.AsKeyword(), accessContext.PrincipalId.AsKeyword()];

            if(accessContext.GroupIds is not null) {
                keys.AddRange(accessContext.GroupIds.Select(groupId => groupId.AsKeyword()));
            }

            searchQuery.And().GroupedOr([SystemFields.Protection], keys.ToArray());
        }
    }

    private static Document? MapToDocument(ISearchResult item) {
        var objectTypeString = item.Values.GetValueOrDefault(ExamineFieldNames.CategoryFieldName);

        _ = Enum.TryParse(objectTypeString, out UmbracoObjectTypes umbracoObjectType);

        if(Guid.TryParse(item.Id, out Guid guidId)) {
            return new Document(guidId, umbracoObjectType);
        }

        // The id of an item may be appended with _{culture_{segment}, so strip those and map to guid.
        var indexofUnderscore = item.Id.IndexOf('_', StringComparison.Ordinal);
        var idWithOutCulture = item.Id.Remove(indexofUnderscore);
        return Guid.TryParse(idWithOutCulture, out Guid idWithoutCultureGuid)
            ? new Document(idWithoutCultureGuid, umbracoObjectType)
            : null;
    }

    private void EnsureFieldAnalyzersLoaded(IIndex index) {
        if(InitializedIndexes(this).TryGetValue(index.Name, out _)) {
            return;
        }

        // Doing a search forces Examine to load per-field analyzers (e.g. KeywordAnalyzer for
        // RawStringType fields). Without this, the first GroupedOr/Field calls after startup
        // use the default StandardAnalyzer, which tokenizes values on hyphens and other
        // punctuation — causing 0 results on keyword fields that store raw values.
        index.Searcher.Search(string.Empty, QueryOptions.SkipTake(0, 0));
        InitializedIndexes(this).TryAdd(index.Name, true);
    }

    private static class SystemFields {
        public const string Protection = "Sys_Protection";
    }
}
