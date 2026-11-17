using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Core.Web;

namespace Shorthand.UmbracoHelpers;

public class FriendlyUrlHelperHelper : IFriendlyUrlHelperHelper {
    private readonly IUmbracoContextAccessor _umbracoContextAccessor;
    private readonly IDataProtectionProvider _dataProtectionProvider;

    public FriendlyUrlHelperHelper(
            IUmbracoContextAccessor umbracoContextAccessor,
            IDataProtectionProvider dataProtectionProvider) {
        _umbracoContextAccessor = umbracoContextAccessor;
        _dataProtectionProvider = dataProtectionProvider;
    }

    public string SurfaceAction(IUrlHelper url, string action, string controllerName)
        => url.SurfaceAction(_umbracoContextAccessor.GetRequiredUmbracoContext(), _dataProtectionProvider, action, controllerName);

    public string SurfaceAction(IUrlHelper url, string action, string controllerName, object additionalRouteVals)
        => url.SurfaceAction(_umbracoContextAccessor.GetRequiredUmbracoContext(), _dataProtectionProvider, action, controllerName, additionalRouteVals);

    public string SurfaceAction(IUrlHelper url, string action, string controllerName, string area, object additionalRouteVals)
        => url.SurfaceAction(_umbracoContextAccessor.GetRequiredUmbracoContext(), _dataProtectionProvider, action, controllerName, area, additionalRouteVals);
}
