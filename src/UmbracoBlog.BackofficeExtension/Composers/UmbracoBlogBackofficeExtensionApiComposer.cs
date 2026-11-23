using Microsoft.OpenApi;
using Umbraco.Cms.Api.Common.OpenApi;
using Umbraco.Cms.Api.Management.OpenApi;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;

namespace UmbracoBlog.BackofficeExtension.Composers;

public class UmbracoBlogBackofficeExtensionApiComposer : IComposer {
    public void Compose(IUmbracoBuilder builder) =>

        // See https://docs.umbraco.com/umbraco-cms/tutorials/creating-a-backoffice-api (and its sub-pages) for
        // guidance on customizing this document.
        builder.AddBackOfficeOpenApiDocument(
            Constants.ApiName,
            document => document
                .WithTitle("Umbraco Blog Backoffice Extension Backoffice API")
                .WithBackOfficeAuthentication()
                .WithJsonOptions(Umbraco.Cms.Core.Constants.JsonOptionsNames.BackOffice)
                .ConfigureOpenApiOptions(options =>
                    options.AddDocumentTransformer((doc, _, _) => {
                        doc.Info.Version = "1.0";
                        doc.Info.Contact = new OpenApiContact {
                            Name = "Karl-Johan Sjögren",
                            Url = new Uri("https://github.com/karl-sjogren")
                        };
                        return Task.CompletedTask;
                    })));
}
