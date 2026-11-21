using Microsoft.AspNetCore.Mvc.Razor;

namespace UmbracoBlog.Umbraco.Mvc;

public class FeatureViewLocationExpander : IViewLocationExpander {
    public void PopulateValues(ViewLocationExpanderContext context) {
        // If we don't set a value here any views with the same name within two different features
        // will only resolve to the first one found. This cache works based on the Values collection
        // so by adding a new value to seperate things here we can ensure that the cache is unique.
        // More info: https://stackoverflow.com/questions/36802661/what-is-iviewlocationexpander-populatevalues-for-in-asp-net-core-mvc
        context.Values["CacheByAction"] = context.ActionContext.ActionDescriptor.DisplayName;
    }

    public IEnumerable<string> ExpandViewLocations(
            ViewLocationExpanderContext context,
            IEnumerable<string> viewLocations) {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(viewLocations);

        var actionDescriptor = context.ActionContext.ActionDescriptor;
        var viewName = context.ViewName;

        var featureName = actionDescriptor.Properties.TryGetValue("feature", out var value)
            ? value as string
            : string.Empty;

        foreach(var location in viewLocations) {
            yield return location
                .Replace("{feature}", featureName, StringComparison.OrdinalIgnoreCase)
                .Replace("{0}", viewName, StringComparison.OrdinalIgnoreCase);
        }
    }
}
