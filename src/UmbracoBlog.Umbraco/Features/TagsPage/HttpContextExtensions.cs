namespace UmbracoBlog.Umbraco.Features.TagsPage;

public static class HttpContextExtensions {
    private const string _tagRouteKey = "UmbracoBlog.TagsPage.Tag";

    extension(HttpContext context) {
        public string? GetTagSlug() =>
            context.Items.TryGetValue(_tagRouteKey, out var tag) && tag is string tagSlug ? tagSlug : null;

        public bool IsTagDetailsRequest() =>
            context.Items.ContainsKey(_tagRouteKey);

        public void SetTagSlug(string tagSlug) =>
            context.Items[_tagRouteKey] = tagSlug;
    }
}
