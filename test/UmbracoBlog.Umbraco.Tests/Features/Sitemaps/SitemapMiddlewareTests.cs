using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Umbraco.Cms.Web.Common.Routing;
using UmbracoBlog.Umbraco.Features.Sitemaps;

namespace UmbracoBlog.Umbraco.Tests.Features.Sitemaps;

public class SitemapMiddlewareTests {
    [Theory]
    [InlineData("/SITEMAP.XML", true)]
    [InlineData("/other.xml", true)]
    [InlineData("/asset.xml", false)]
    public void Composer_OptsSitemapIntoServerSideRequests_WithoutOverridingExistingRules(string path, bool expected) {
        // Arrange
        var services = new ServiceCollection();
        services.Configure<UmbracoRequestOptions>(options =>
            options.HandleAsServerSideRequest = request => request.Path == "/other.xml");

        var builder = A.Fake<IUmbracoBuilder>();
        A.CallTo(() => builder.Services).Returns(services);

        new SitemapComposer().Compose(builder);

        using var provider = services.BuildServiceProvider();
        var options = provider.GetRequiredService<IOptions<UmbracoRequestOptions>>().Value;
        var context = new DefaultHttpContext();

        // Act
        context.Request.Path = path;

        // Assert
        options.HandleAsServerSideRequest(context.Request).ShouldBe(expected);
    }

    [Fact]
    public async Task InvokeAsync_OtherRequest_PassesThroughWithoutUmbracoContextAsync() {
        // Arrange
        var generator = A.Fake<ISitemapGenerator>();
        var middleware = new SitemapMiddleware(generator);

        var context = new DefaultHttpContext();
        context.Request.Path = "/asset.xml";
        var calledNext = false;

        // Act
        await middleware.InvokeAsync(context, _ => {
            calledNext = true;
            return Task.CompletedTask;
        });

        // Assert
        calledNext.ShouldBeTrue();
        A.CallTo(() => generator.GenerateSitemapAsync(A<Stream>._, A<CancellationToken>._)).MustNotHaveHappened();
    }

    [Fact]
    public async Task InvokeAsync_SitemapRequestWithContext_GeneratesSitemapAsync() {
        // Arrange
        var generator = A.Fake<ISitemapGenerator>();
        var middleware = new SitemapMiddleware(generator);

        var context = new DefaultHttpContext();
        context.Request.Path = "/sitemap.xml";
        var calledNext = false;

        // Act
        await middleware.InvokeAsync(context, _ => {
            calledNext = true;
            return Task.CompletedTask;
        });

        // Assert
        calledNext.ShouldBeFalse();
        context.Response.ContentType.ShouldBe("application/xml; charset=utf-8");
        A.CallTo(() => generator.GenerateSitemapAsync(context.Response.Body, context.RequestAborted)).MustHaveHappenedOnceExactly();
    }
}
