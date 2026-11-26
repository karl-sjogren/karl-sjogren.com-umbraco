using UmbracoBlog.Core.ViewModels;

namespace UmbracoBlog.Umbraco.Features.SearchPage;

public class SearchPageViewModel : ViewModelBase<Core.Documents.SearchPage> {
    public string SearchQuery { get; set; }
    public ICollection<SearchPageHitViewModel> SearchHits { get; set; }
    public long TotalHits { get; set; }

    public SearchPageViewModel(Core.Documents.SearchPage content, string searchQuery, ICollection<SearchPageHitViewModel> searchHits, long totalHits)
            : base(content) {
        SearchQuery = searchQuery;
        SearchHits = searchHits;
        TotalHits = totalHits;
    }
}

public record SearchPageHitViewModel(string Title, string Url, string Type);
