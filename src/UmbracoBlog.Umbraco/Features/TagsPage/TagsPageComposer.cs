using Umbraco.Cms.Core.Composing;

namespace UmbracoBlog.Umbraco.Features.TagsPage;

public class TagsPageComposer : IComposer {
    public void Compose(IUmbracoBuilder builder) {
        builder.ContentFinders().Append<TagContentFinder>();
    }
}
