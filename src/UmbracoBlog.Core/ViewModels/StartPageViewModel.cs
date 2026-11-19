using UmbracoBlog.Core.Documents;

namespace UmbracoBlog.Core.ViewModels;

public class StartPageViewModel : ViewModelBase<StartPage> {
    public ICollection<BlogPost> BlogPosts { get; set; }

    public StartPageViewModel(StartPage content, ICollection<BlogPost> blogPosts)
        : base(content) {
        BlogPosts = blogPosts;
    }
}

public class ViewModelBase<T> where T : class {
    public T Content { get; set; }

    public ViewModelBase(T content) {
        Content = content;
    }
}
