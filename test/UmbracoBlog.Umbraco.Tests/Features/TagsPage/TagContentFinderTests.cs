using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Umbraco.Cms.Core.Configuration.Models;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PublishedCache;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Web;
using UmbracoBlog.Umbraco.Features.TagsPage;

namespace UmbracoBlog.Umbraco.Tests.Features.TagsPage;

public class TagContentFinderTests {
    private readonly IUmbracoContextAccessor _contextAccessor = A.Fake<IUmbracoContextAccessor>();
    private readonly IUmbracoContext _umbracoContext = A.Fake<IUmbracoContext>();
    private readonly DefaultHttpContext _httpContext = new();
    private readonly IDocumentUrlService _documentUrlService = A.Fake<IDocumentUrlService>();
    private readonly IPublishedContentCache _contentCache = A.Fake<IPublishedContentCache>();
    private readonly IOptionsMonitor<WebRoutingSettings> _routingSettings = A.Fake<IOptionsMonitor<WebRoutingSettings>>();
    private readonly Guid _pageKey = Guid.NewGuid();

    public TagContentFinderTests() {
        IUmbracoContext? context = null;
        A.CallTo(() => _contextAccessor.TryGetUmbracoContext(out context))
            .Returns(true)
            .AssignsOutAndRefParameters(_umbracoContext);
        A.CallTo(() => _routingSettings.CurrentValue).Returns(new WebRoutingSettings());
    }

    [Theory]
    [InlineData("/tags/cool-tag", "/tags", "cool-tag")]
    [InlineData("/nested/topics/cool-tag/", "/nested/topics", "cool-tag")]
    public async Task TryFindContent_TagUnderTagsPage_SetsContentAndTagAsync(string path, string parentPath, string tag) {
        var page = SetupPage(parentPath, Core.Documents.TagsPage.ModelTypeAlias);
        var request = CreateRequest(path);

        var found = await CreateFinder().TryFindContent(request);

        found.ShouldBeTrue();
        request.PublishedContent.ShouldBeSameAs(page);
        _httpContext.Items.Values.ShouldContain(tag);
    }

    [Fact]
    public async Task TryFindContent_DomainPath_ResolvesRelativeToDomainAsync() {
        var page = SetupPage("/tags", Core.Documents.TagsPage.ModelTypeAlias, 42, "sv-SE");
        var request = CreateRequest("/sv/tags/cool-tag");
        request.SetDomain(new DomainAndUri(
            new Domain(1, "example.com/sv", 42, "sv-SE", false, 0), request.Uri));

        var found = await CreateFinder().TryFindContent(request);

        found.ShouldBeTrue();
        request.PublishedContent.ShouldBeSameAs(page);
        _httpContext.Items.Values.ShouldContain("cool-tag");
    }

    [Fact]
    public async Task TryFindContent_ParentIsNotTagsPage_DoesNotHandleRequestAsync() {
        SetupPage("/other", "contentPage");
        var request = CreateRequest("/other/cool-tag");

        var found = await CreateFinder().TryFindContent(request);

        found.ShouldBeFalse();
        request.PublishedContent.ShouldBeNull();
        _httpContext.Items.ShouldBeEmpty();
    }

    [Fact]
    public async Task TryFindContent_ParentDoesNotExist_DoesNotHandleRequestAsync() {
        var request = CreateRequest("/tags/cool-tag/extra");

        var found = await CreateFinder().TryFindContent(request);

        found.ShouldBeFalse();
        request.PublishedContent.ShouldBeNull();
        _httpContext.Items.ShouldBeEmpty();
    }

    [Fact]
    public async Task TryFindContent_StrictDomainMatchingWithoutDomain_DoesNotHandleRequestAsync() {
        A.CallTo(() => _routingSettings.CurrentValue).Returns(new WebRoutingSettings { UseStrictDomainMatching = true });
        SetupPage("/tags", Core.Documents.TagsPage.ModelTypeAlias);
        var request = CreateRequest("/tags/cool-tag");

        var found = await CreateFinder().TryFindContent(request);

        found.ShouldBeFalse();
        request.PublishedContent.ShouldBeNull();
    }

    [Fact]
    public async Task TryFindContent_Preview_UsesPreviewRoutesAndContentAsync() {
        A.CallTo(() => _umbracoContext.InPreviewMode).Returns(true);
        var page = SetupPage("/tags", Core.Documents.TagsPage.ModelTypeAlias, preview: true);
        var request = CreateRequest("/tags/cool-tag");

        var found = await CreateFinder().TryFindContent(request);

        found.ShouldBeTrue();
        request.PublishedContent.ShouldBeSameAs(page);
    }

    private TagContentFinder CreateFinder() => new(
        _contextAccessor,
        new HttpContextAccessor { HttpContext = _httpContext },
        _documentUrlService,
        _contentCache,
        _routingSettings);

    private IPublishedContent SetupPage(string route, string alias, int? domainId = null, string? culture = null, bool preview = false) {
        var page = A.Fake<IPublishedContent>();
        var contentType = A.Fake<IPublishedContentType>();
        A.CallTo(() => contentType.Alias).Returns(alias);
        A.CallTo(() => page.ContentType).Returns(contentType);
        A.CallTo(() => _documentUrlService.GetDocumentKeyByRoute(route, culture, domainId, preview)).Returns(_pageKey);
        A.CallTo(() => _contentCache.GetById(preview, _pageKey)).Returns(page);
        return page;
    }

    private static PublishedRequestBuilder CreateRequest(string path) =>
        new(new Uri("https://example.com" + path), A.Fake<ITemplateService>());
}
