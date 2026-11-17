namespace Shorthand.UmbracoHelpers;

public static class FriendlyHelperServiceCollectionExtensions {
    public static IServiceCollection AddFriendlyHelpers(this IServiceCollection services) {
        services.AddScoped<IFriendlyImageCropperTemplateHelper, FriendlyImageCropperTemplateHelper>();
        services.AddScoped<IFriendlyPublishedContentHelper, FriendlyPublishedContentHelper>();
        services.AddScoped<IFriendlyPublishedElementHelper, FriendlyPublishedElementHelper>();
        services.AddScoped<IFriendlyUrlHelperHelper, FriendlyUrlHelperHelper>();
        return services;
    }
}
