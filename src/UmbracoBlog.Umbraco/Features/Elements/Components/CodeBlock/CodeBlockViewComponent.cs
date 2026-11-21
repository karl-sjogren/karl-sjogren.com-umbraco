using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace UmbracoBlog.Umbraco.Features.Elements.Components.CodeBlock;

public class CodeBlockViewComponent : ViewComponent {
    public Task<IViewComponentResult> InvokeAsync(IPublishedElement element) {
        return Task.FromResult(View("Default", element as Core.Elements.CodeBlock) as IViewComponentResult);
    }
}
