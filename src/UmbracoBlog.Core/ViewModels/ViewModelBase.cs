namespace UmbracoBlog.Core.ViewModels;

public abstract class ViewModelBase<T> where T : class {
    public T Content { get; set; }

    public ViewModelBase(T content) {
        Content = content;
    }
}
