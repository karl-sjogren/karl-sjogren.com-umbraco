using Examine.Search;
using Umbraco.Cms.Search.Core.Models.Searching;

using UmbracoSearchResult = Umbraco.Cms.Search.Core.Models.Searching.SearchResult;

namespace UmbracoBlog.Umbraco.Features.Search;

public interface IUmbracoBlogSearcher {
    UmbracoSearchResult SearchWithMinimumShouldMatch(string indexAlias, AccessContext? accessContext, string field, string[] values, int minimumNumberShouldMatch, int skip, int take, Func<IBooleanOperation, IBooleanOperation>? configureQuery = null);
}
