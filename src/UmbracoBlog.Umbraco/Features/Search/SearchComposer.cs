using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Infrastructure.Examine;
using Umbraco.Cms.Search.BackOffice.DependencyInjection;
using Umbraco.Cms.Search.Core.DependencyInjection;
using Umbraco.Cms.Search.Core.Services.ContentIndexing;
using Umbraco.Cms.Search.Provider.Examine.Configuration;
using Umbraco.Cms.Search.Provider.Examine.DependencyInjection;

namespace UmbracoBlog.Umbraco.Features.Search;

[ComposeAfter(typeof(AddExamineComposer))]
public class SearchComposer : IComposer {
    public void Compose(IUmbracoBuilder builder) {
        builder
            .AddSearchCore()
            .AddExamineSearchProvider()
            .AddBackOfficeSearch()
            .DisableDefaultExamineIndexes();

        builder.Services.AddSingleton<IContentIndexer, ContentTypeAliasContentIndexer>();

        // Uncomment this to clear that nasty cache that prevents us from doing stuff easily
        //builder.Services.AddHostedService<CleanSearchCachesBackgroundService>();

        builder.Services.Configure<FieldOptions>(options => options.Fields = [
            new() { PropertyName = Core.Documents.BlogPost.PropertyAliases.Tags, FieldValues = FieldValues.Keywords, Facetable = true },
        ]);
    }
}
