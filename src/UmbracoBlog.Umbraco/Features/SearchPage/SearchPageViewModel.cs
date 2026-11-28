using UmbracoBlog.Core.ViewModels;

namespace UmbracoBlog.Umbraco.Features.SearchPage;

public class SearchPageViewModel : ViewModelBase<Core.Documents.SearchPage> {
    public string SearchQuery { get; set; }
    public ICollection<SearchPageHitViewModel> SearchHits { get; set; }

    public required int PageNumber { get; set; }
    public required int PageSize { get; set; }
    public required int TotalHits { get; set; }

    public SearchPageViewModel(Core.Documents.SearchPage content, string searchQuery, ICollection<SearchPageHitViewModel> searchHits)
            : base(content) {
        SearchQuery = searchQuery;
        SearchHits = searchHits;
    }
}

public record SearchPageHitViewModel(string Title, string Url, string Type);
