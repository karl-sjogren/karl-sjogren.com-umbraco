using UmbracoBlog.Core.Documents;

namespace UmbracoBlog.Core.ViewModels;

public class StartPageViewModel : ViewModelBase<StartPage> {
    public ICollection<BlogPost> BlogPosts { get; set; }

    public StartPageViewModel(StartPage content, ICollection<BlogPost> blogPosts)
            : base(content) {
        BlogPosts = blogPosts;
    }
}
