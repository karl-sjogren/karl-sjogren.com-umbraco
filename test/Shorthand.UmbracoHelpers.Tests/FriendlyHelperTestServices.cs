using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Umbraco.Cms.Core.Configuration.Models;
using Umbraco.Cms.Core.Media;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PublishedCache;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Serialization;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Services.Navigation;
using Umbraco.Cms.Core.Web;

namespace Shorthand.UmbracoHelpers.Tests;

internal static class FriendlyHelperTestServices {
    public static ServiceCollection Create() {
        var services = new ServiceCollection();

        services.AddLogging();
        services.AddSingleton(A.Fake<IImageUrlGenerator>());
        services.AddSingleton(A.Fake<IJsonSerializer>());
        services.AddSingleton(A.Fake<IPublishedValueFallback>());
        services.AddSingleton(A.Fake<IPublishedUrlProvider>());
        services.AddSingleton(A.Fake<IVariationContextAccessor>());
        services.AddSingleton(A.Fake<IDomainCache>());
        services.AddSingleton(A.Fake<IDocumentNavigationQueryService>());
        services.AddSingleton(A.Fake<IMediaNavigationQueryService>());
        services.AddSingleton(A.Fake<IPublishedContentStatusFilteringService>());
        services.AddSingleton(A.Fake<IPublishedMediaStatusFilteringService>());
        services.AddSingleton(A.Fake<IPublishedModelFactory>());
        services.AddSingleton(A.Fake<IUmbracoContextAccessor>());
        services.AddSingleton(A.Fake<ISiteDomainMapper>());
        services.AddSingleton(A.Fake<ITemplateService>());
        services.AddSingleton<IOptions<WebRoutingSettings>>(Options.Create(new WebRoutingSettings()));
        services.AddSingleton(A.Fake<IContentTypeService>());
        services.AddSingleton(A.Fake<IDocumentUrlService>());
        services.AddSingleton(A.Fake<IUserService>());
        services.AddSingleton<IDataProtectionProvider>(new EphemeralDataProtectionProvider());

        return services;
    }
}
