using UmbracoBlog.Core.ViewModels;

namespace UmbracoBlog.Umbraco.Features.StartPage;

public class StartPageViewModel : ViewModelBase<Core.Documents.StartPage> {
    public ICollection<Core.Documents.BlogPost> BlogPosts { get; set; }

    public StartPageViewModel(Core.Documents.StartPage content, ICollection<Core.Documents.BlogPost> blogPosts)
            : base(content) {
        BlogPosts = blogPosts;
    }
}
