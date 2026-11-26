using UmbracoBlog.Core.ViewModels;

namespace UmbracoBlog.Umbraco.Features.TagsPage;

public class TagsPageViewModel : ViewModelBase<Core.Documents.TagsPage> {
    public ICollection<TagsPageHitViewModel> Tags { get; }

    public TagsPageViewModel(Core.Documents.TagsPage content, ICollection<TagsPageHitViewModel> tags)
        : base(content) {
        Tags = tags;
    }
}

public record TagsPageHitViewModel(string Name, float Count);
