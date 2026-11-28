namespace UmbracoBlog.Umbraco.Features.Components.Pagination;

public record PaginationViewModel {
    public required ICollection<PaginationLinkViewModel> PageNumberLinks { get; init; }
}

public record PaginationLinkViewModel {
    public required string Label { get; init; }
    public required string CssClass { get; init; }
    public required string Url { get; init; }
    public required bool IsCurrent { get; init; }
}
