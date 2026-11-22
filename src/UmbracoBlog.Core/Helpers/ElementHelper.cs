using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.DependencyInjection;
using Umbraco.Cms.Core.Models.PublishedContent;
using UmbracoBlog.Core.Contracts;

namespace UmbracoBlog.Core.Helpers;

public class ElementHelper : IElementHelper, IViewContextAware {
    private readonly IViewComponentDescriptorCollectionProvider _descriptorProvider;
    private readonly ILogger<ElementHelper> _logger;
    private ViewContext? _viewContext = null;

    public ElementHelper(IViewComponentDescriptorCollectionProvider descriptorProvider, ILogger<ElementHelper> logger) {
        _descriptorProvider = descriptorProvider;
        _logger = logger;
    }

    public void Contextualize(ViewContext viewContext) {
        _viewContext = viewContext;
    }

    /// <inheritdoc />
    public string GetViewComponentForElement(IPublishedElement element) {
        ArgumentNullException.ThrowIfNull(element);

        var components = _descriptorProvider.ViewComponents.Items;
        var elementAlias = element.ContentType.Alias;

        var matchingComponents = components
            .Where(c => c.ShortName.Equals(elementAlias, StringComparison.OrdinalIgnoreCase))
            .ToList();

        if(matchingComponents.Count == 0) {
            throw new InvalidOperationException($"No view component found for element alias '{elementAlias}'.");
        }

        if(matchingComponents.Count > 1) {
            throw new InvalidOperationException($"Multiple view components found for element alias '{elementAlias}'. View components: {string.Join(", ", matchingComponents.Select(c => c.ShortName))}");
        }

        return matchingComponents[0].ShortName;
    }

    /// <inheritdoc />
    public async Task<IHtmlContent> RenderViewComponentForElementAsync(IPublishedElement element) {
        ArgumentNullException.ThrowIfNull(element);
        ArgumentNullException.ThrowIfNull(_viewContext);

        var viewComponentName = GetViewComponentForElement(element);
        var viewComponentHelper = _viewContext.HttpContext.RequestServices.GetRequiredService<IViewComponentHelper>();
        ((IViewContextAware)viewComponentHelper).Contextualize(_viewContext);

        return await viewComponentHelper.InvokeAsync(viewComponentName, new { element });
    }
}
