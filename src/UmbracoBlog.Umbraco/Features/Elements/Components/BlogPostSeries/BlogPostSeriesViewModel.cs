namespace UmbracoBlog.Umbraco.Features.Elements.Components.BlogPostSeries;

public class BlogPostSeriesViewModel {
    public required string Title { get; set; }
    public required ICollection<BlogPostSeriesLinkViewModel> Parts { get; set; }
}

public record BlogPostSeriesLinkViewModel(string Title, DateTime PublishDate, string Url, bool IsCurrentPost);
