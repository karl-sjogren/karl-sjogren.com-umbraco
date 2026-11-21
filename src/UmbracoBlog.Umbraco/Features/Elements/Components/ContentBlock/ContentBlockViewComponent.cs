using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace UmbracoBlog.Umbraco.Features.Elements.Components.ContentBlock;

public class ContentBlockViewComponent : ViewComponent {
    public Task<IViewComponentResult> InvokeAsync(IPublishedElement element) {
        return Task.FromResult(View("Default", element as Core.Elements.ContentBlock) as IViewComponentResult);
    }
}
