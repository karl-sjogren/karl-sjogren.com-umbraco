using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace UmbracoBlog.Core.Elements;

public class JavascriptCodeBlockViewComponent : ViewComponent {
    public Task<IViewComponentResult> InvokeAsync(IPublishedElement element) {
        return Task.FromResult(View(element as JavascriptCodeBlock) as IViewComponentResult);
    }
}
