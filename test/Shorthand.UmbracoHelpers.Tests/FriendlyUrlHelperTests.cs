using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Core.Web;

namespace Shorthand.UmbracoHelpers.Tests;

public class FriendlyUrlHelperHelperTests {
    [Fact]
    public void SurfaceActionUsesTheCurrentContextOnEveryCall() {
        // Arrange
        var accessor = A.Fake<IUmbracoContextAccessor>();
        var first = A.Fake<IUmbracoContext>();
        var second = A.Fake<IUmbracoContext>();

        A.CallTo(() => first.OriginalRequestUrl).Returns(new Uri("https://example.test/first"));
        A.CallTo(() => second.OriginalRequestUrl).Returns(new Uri("https://example.test/second"));

        IUmbracoContext? current = first;
        A.CallTo(() => accessor.TryGetUmbracoContext(out current))
            .Returns(true).AssignsOutAndRefParameters(first)
            .Once().Then.Returns(true).AssignsOutAndRefParameters(second);

        var helper = new FriendlyUrlHelperHelper(accessor, new EphemeralDataProtectionProvider());
        var url = A.Fake<IUrlHelper>();

        // Act & assert
        helper.SurfaceAction(url, "Save", "Contact").ShouldStartWith("/first?ufprt=");
        helper.SurfaceAction(url, "Save", "Contact").ShouldStartWith("/second?ufprt=");
    }
}
