using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Services;

namespace Shorthand.UmbracoHelpers.Tests;

public class FriendlyPublishedElementHelperTests {
    [Fact]
    public void ElementValueForwardsCultureAndSegment() {
        // Arrange
        var fallback = A.Fake<IPublishedValueFallback>();
        var helper = new FriendlyPublishedElementHelper(fallback, A.Fake<IVariationContextAccessor>(), A.Fake<IUserService>());
        var element = A.Fake<IPublishedElement>();
        var property = A.Fake<IPublishedProperty>();

        A.CallTo(() => element.GetProperty("title")).Returns(property);
        A.CallTo(() => property.HasValue("sv-SE", "desktop")).Returns(true);
        A.CallTo(() => property.GetValue("sv-SE", "desktop")).Returns("Hej");

        // Act & assert
        helper.Value(element, "title", "sv-SE", "desktop").ShouldBe("Hej");

        // Assert
        A.CallTo(() => property.GetValue("sv-SE", "desktop")).MustHaveHappenedOnceExactly();
    }
}
