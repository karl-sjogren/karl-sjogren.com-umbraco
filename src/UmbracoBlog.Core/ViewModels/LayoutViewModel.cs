using UmbracoBlog.Core.Elements;

namespace UmbracoBlog.Core.ViewModels;

public class LayoutViewModel {
    public required string MetaTitle { get; init; }
    public required string Url { get; init; }
    public required string CanonicalUrl { get; init; }
    public string? SeoTitle { get; set; }
    public string? MetaDescription { get; init; }
    public bool HideSidebarSearch { get; set; }
    public bool BlockSearchEngineIndexing { get; init; }
    public required SiteSettingsElement? SiteSettings { get; init; }
    public required int CopyrightYear { get; init; }

    public string Title => string.IsNullOrWhiteSpace(SeoTitle) ? MetaTitle : SeoTitle;
}

public class LayoutViewModel<TViewModel> : LayoutViewModel where TViewModel : class {
    public LayoutViewModel(TViewModel viewModel) {
        ViewModel = viewModel;
    }

    public TViewModel ViewModel { get; init; }
}
