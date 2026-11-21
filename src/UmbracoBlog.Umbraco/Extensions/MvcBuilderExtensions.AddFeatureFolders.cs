using UmbracoBlog.Umbraco.Mvc;

namespace UmbracoBlog.Umbraco.Extensions;

public static partial class MvcBuilderExtensions {
    public static IMvcBuilder AddFeatureFolders(this IMvcBuilder services) {
        if(services == null) {
            throw new ArgumentNullException(nameof(services));
        }

        services.AddMvcOptions(o => o.Conventions.Add(new ControllerFeatureConvention()))
            .AddRazorOptions(o => {
                o.ViewLocationFormats.Add("{feature}/Views/{0}.cshtml");
                o.ViewLocationFormats.Add("{feature}/{0}.cshtml");
                o.ViewLocationFormats.Add("/Features/Shared/{0}.cshtml");
                o.ViewLocationFormats.Add("/Features/Elements/{0}.cshtml");
                o.ViewLocationFormats.Add("/Features/{0}.cshtml");

                o.ViewLocationExpanders.Add(new FeatureViewLocationExpander());
            });

        return services;
    }
}
