using Microsoft.Extensions.DependencyInjection;

namespace Shorthand.UmbracoHelpers.Tests;

public class FriendlyHelperServiceCollectionExtensionsTests {
    [Fact]
    public void RegistrationsResolveInScopes() {
        // Arrange
        var services = FriendlyHelperTestServices.Create();
        services.AddFriendlyHelpers();

        using var provider = services.BuildServiceProvider(new ServiceProviderOptions {
            ValidateScopes = true,
            ValidateOnBuild = true,
        });
        using var first = provider.CreateScope();
        using var second = provider.CreateScope();

        // Act & assert
        first.ServiceProvider.GetRequiredService<IFriendlyImageCropperTemplateHelper>()
            .ShouldBeOfType<FriendlyImageCropperTemplateHelper>();
        first.ServiceProvider.GetRequiredService<IFriendlyPublishedElementHelper>()
            .ShouldBeOfType<FriendlyPublishedElementHelper>();
        first.ServiceProvider.GetRequiredService<IFriendlyUrlHelperHelper>()
            .ShouldBeOfType<FriendlyUrlHelperHelper>();

        var contentHelper = first.ServiceProvider.GetRequiredService<IFriendlyPublishedContentHelper>();

        first.ServiceProvider.GetRequiredService<IFriendlyPublishedContentHelper>().ShouldBeSameAs(contentHelper);
        second.ServiceProvider.GetRequiredService<IFriendlyPublishedContentHelper>().ShouldNotBeSameAs(contentHelper);
    }
}
