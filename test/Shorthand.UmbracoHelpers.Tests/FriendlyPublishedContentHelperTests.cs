using Examine;
using Microsoft.Extensions.DependencyInjection;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Models.Membership;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Services.Navigation;
using Umbraco.Cms.Core.Web;

namespace Shorthand.UmbracoHelpers.Tests;

public class FriendlyPublishedContentHelperTests {
    [Theory]
    [InlineData(true, null)]
    [InlineData(true, "CustomIndex")]
    [InlineData(false, null)]
    [InlineData(false, "CustomIndex")]
    public void SearchUsesInjectedExamineManager(bool children, string? indexName) {
        var services = FriendlyHelperTestServices.Create();
        var examine = A.Fake<IExamineManager>();
        services.AddSingleton(examine);
        services.AddFriendlyHelpers();
        using var provider = services.BuildServiceProvider();
        using var scope = provider.CreateScope();
        var helper = scope.ServiceProvider.GetRequiredService<IFriendlyPublishedContentHelper>();
        var content = A.Fake<IPublishedContent>();
        IIndex index = null!;

        Should.Throw<InvalidOperationException>(() => {
            if(children) {
                helper.SearchChildren(content, "needle", indexName);
            } else {
                helper.SearchDescendants(content, "needle", indexName);
            }
        });

        A.CallTo(() => examine.TryGetIndex(indexName ?? Constants.UmbracoIndexes.ExternalIndexName, out index))
            .MustHaveHappenedOnceExactly();
    }

    [Theory]
    [InlineData(PublishedItemType.Content)]
    [InlineData(PublishedItemType.Media)]
    public void ChildrenUseTheMatchingNavigationAndStatusServices(PublishedItemType itemType) {
        // Arrange
        var services = FriendlyHelperTestServices.Create();
        var documents = A.Fake<IDocumentNavigationQueryService>();
        var media = A.Fake<IMediaNavigationQueryService>();
        var documentStatus = A.Fake<IPublishedContentStatusFilteringService>();
        var mediaStatus = A.Fake<IPublishedMediaStatusFilteringService>();

        services.AddSingleton(documents);
        services.AddSingleton(media);
        services.AddSingleton(documentStatus);
        services.AddSingleton(mediaStatus);
        services.AddFriendlyHelpers();

        using var provider = services.BuildServiceProvider();
        using var scope = provider.CreateScope();
        var helper = scope.ServiceProvider.GetRequiredService<IFriendlyPublishedContentHelper>();

        var content = A.Fake<IPublishedContent>();
        var contentType = A.Fake<IPublishedContentType>();
        var key = Guid.NewGuid();
        var childKey = Guid.NewGuid();
        IEnumerable<Guid> keys = [childKey];
        var child = A.Fake<IPublishedContent>();

        A.CallTo(() => content.ContentType).Returns(contentType);
        A.CallTo(() => contentType.ItemType).Returns(itemType);
        A.CallTo(() => content.Key).Returns(key);
        A.CallTo(() => documents.TryGetChildrenKeys(key, out keys)).Returns(true).AssignsOutAndRefParameters(new[] { childKey });
        A.CallTo(() => media.TryGetChildrenKeys(key, out keys)).Returns(true).AssignsOutAndRefParameters(new[] { childKey });
        A.CallTo(() => documentStatus.FilterAvailable(A<IEnumerable<Guid>>._, "sv-SE")).Returns([child]);
        A.CallTo(() => mediaStatus.FilterAvailable(A<IEnumerable<Guid>>._, "sv-SE")).Returns([child]);

        // Act & assert
        helper.Children(content, "sv-SE").ShouldHaveSingleItem().ShouldBeSameAs(child);

        // Assert
        if(itemType == PublishedItemType.Content) {
            A.CallTo(() => documents.TryGetChildrenKeys(key, out keys)).MustHaveHappenedOnceExactly();
            A.CallTo(() => documentStatus.FilterAvailable(A<IEnumerable<Guid>>._, "sv-SE")).MustHaveHappenedOnceExactly();
            A.CallTo(() => media.TryGetChildrenKeys(key, out keys)).MustNotHaveHappened();
        } else {
            A.CallTo(() => media.TryGetChildrenKeys(key, out keys)).MustHaveHappenedOnceExactly();
            A.CallTo(() => mediaStatus.FilterAvailable(A<IEnumerable<Guid>>._, "sv-SE")).MustHaveHappenedOnceExactly();
            A.CallTo(() => documents.TryGetChildrenKeys(key, out keys)).MustNotHaveHappened();
        }
    }

    [Fact]
    public void ContentNamesUseInjectedUserService() {
        // Arrange
        var services = FriendlyHelperTestServices.Create();
        var userService = A.Fake<IUserService>();
        var creator = A.Fake<IProfile>();
        var writer = A.Fake<IProfile>();
        var content = A.Fake<IPublishedContent>();

        A.CallTo(() => content.CreatorId).Returns(1);
        A.CallTo(() => content.WriterId).Returns(2);
        A.CallTo(() => creator.Name).Returns("Creator");
        A.CallTo(() => writer.Name).Returns("Writer");
        A.CallTo(() => userService.GetProfileById(1)).Returns(creator);
        A.CallTo(() => userService.GetProfileById(2)).Returns(writer);

        services.AddSingleton(userService);
        services.AddFriendlyHelpers();

        using var provider = services.BuildServiceProvider();
        using var scope = provider.CreateScope();
        var helper = scope.ServiceProvider.GetRequiredService<IFriendlyPublishedContentHelper>();

        // Act & assert
        helper.CreatorName(content).ShouldBe("Creator");
        helper.WriterName(content).ShouldBe("Writer");
    }

    [Fact]
    public void UrlSegmentUsesCurrentCultureAndPreviewState() {
        // Arrange
        var services = FriendlyHelperTestServices.Create();
        var accessor = A.Fake<IUmbracoContextAccessor>();
        var context = A.Fake<IUmbracoContext>();
        var variation = A.Fake<IVariationContextAccessor>();
        var documentUrl = A.Fake<IDocumentUrlService>();
        var content = A.Fake<IPublishedContent>();
        var key = Guid.NewGuid();
        IUmbracoContext? current = context;

        A.CallTo(() => accessor.TryGetUmbracoContext(out current)).Returns(true).AssignsOutAndRefParameters(context);
        A.CallTo(() => context.InPreviewMode).Returns(true);
        A.CallTo(() => variation.VariationContext).Returns(new VariationContext("sv-SE"));
        A.CallTo(() => content.Key).Returns(key);
        A.CallTo(() => documentUrl.GetUrlSegment(key, "sv-SE", true)).Returns("segment");

        services.AddSingleton(accessor);
        services.AddSingleton(variation);
        services.AddSingleton(documentUrl);
        services.AddFriendlyHelpers();

        using var provider = services.BuildServiceProvider();
        using var scope = provider.CreateScope();

        // Act & assert
        scope.ServiceProvider.GetRequiredService<IFriendlyPublishedContentHelper>().UrlSegment(content).ShouldBe("segment");

        // Assert
        A.CallTo(() => documentUrl.GetUrlSegment(key, "sv-SE", true)).MustHaveHappenedOnceExactly();
    }
}
