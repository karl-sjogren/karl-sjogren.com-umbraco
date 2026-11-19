using Microsoft.AspNetCore.Html;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace UmbracoBlog.Core.Contracts;

public interface IElementHelper {
    /// <summary>
    /// Gets the view component name for the specified element.
    /// </summary>
    /// <param name="element">The published element to get the view component for.</param>
    /// <returns>The name of the view component corresponding to the element.</returns>
    string GetViewComponentForElement(IPublishedElement element);

    /// <summary>
    /// Renders the view component for the specified element.
    /// </summary>
    /// <param name="element">The published element to render the view component for.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the rendered HTML content.</returns>
    Task<IHtmlContent> RenderViewComponentForElementAsync(IPublishedElement element);
}
