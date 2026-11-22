using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace UmbracoBlog.Core.Elements;

public class ImageBlockViewComponent : ViewComponent {
    public Task<IViewComponentResult> InvokeAsync(IPublishedElement element) {
        return Task.FromResult(View(element as ImageBlock) as IViewComponentResult);
    }
}
