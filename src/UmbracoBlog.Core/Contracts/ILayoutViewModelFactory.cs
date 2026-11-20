using Umbraco.Cms.Core.Models.PublishedContent;
using UmbracoBlog.Core.ViewModels;

namespace UmbracoBlog.Core.Contracts;

public interface ILayoutViewModelFactory {
    LayoutViewModel<TViewModel> Create<TViewModel>(TViewModel viewModel, IPublishedContent publishedContent) where TViewModel : class;
}
