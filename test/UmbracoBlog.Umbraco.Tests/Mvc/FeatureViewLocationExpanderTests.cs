using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Razor;
using UmbracoBlog.Umbraco.Mvc;

namespace UmbracoBlog.Umbraco.Tests.Mvc;

public class FeatureViewLocationExpanderTests {
    [Fact]
    public void ExpandViewLocations_WhenCalled_ShouldEnumerateFeatureFolders() {
        var expander = new FeatureViewLocationExpander();

        var actionContext = new ActionContext {
            ActionDescriptor = new ActionDescriptor {
                Properties = {
                    ["feature"] = "Features/Header"
                }
            }
        };

        var expanderContext = new ViewLocationExpanderContext(actionContext, "Default", "Application", null, null, true);

        var locations = expander.ExpandViewLocations(expanderContext, [
            "{feature}/Views/{0}.cshtml",
            "{feature}/{0}.cshtml",
            "/Features/Shared/{0}.cshtml",
            "/Features/{0}.cshtml"
        ]).ToArray();

        locations.Length.ShouldBe(4);
        locations.ShouldContain("Features/Header/Views/Default.cshtml");
        locations.ShouldContain("Features/Header/Default.cshtml");
        locations.ShouldContain("/Features/Shared/Default.cshtml");
        locations.ShouldContain("/Features/Default.cshtml");
    }
}
