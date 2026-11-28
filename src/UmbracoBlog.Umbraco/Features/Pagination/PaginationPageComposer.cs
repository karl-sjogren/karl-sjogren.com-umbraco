using Umbraco.Cms.Core.Composing;

namespace UmbracoBlog.Umbraco.Features.Pagination;

public class PaginationPageComposer : IComposer {
    public void Compose(IUmbracoBuilder builder) {
        builder.ContentFinders().Append<PaginationContentFinder>();
    }
}
