using Microsoft.AspNetCore.Mvc;

namespace Shorthand.UmbracoHelpers;

public interface IFriendlyUrlHelperHelper {
    /// <inheritdoc cref="Umbraco.Extensions.FriendlyUrlHelperExtensions.SurfaceAction(IUrlHelper, string, string)"/>
    string SurfaceAction(IUrlHelper url, string action, string controllerName);

    /// <inheritdoc cref="Umbraco.Extensions.FriendlyUrlHelperExtensions.SurfaceAction(IUrlHelper, string, string, object)"/>
    string SurfaceAction(IUrlHelper url, string action, string controllerName, object additionalRouteVals);

    /// <inheritdoc cref="Umbraco.Extensions.FriendlyUrlHelperExtensions.SurfaceAction(IUrlHelper, string, string, string, object)"/>
    string SurfaceAction(IUrlHelper url, string action, string controllerName, string area, object additionalRouteVals);
}
