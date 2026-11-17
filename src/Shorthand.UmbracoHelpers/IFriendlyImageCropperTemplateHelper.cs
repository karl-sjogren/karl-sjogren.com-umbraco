using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PropertyEditors.ValueConverters;

namespace Shorthand.UmbracoHelpers;

public interface IFriendlyImageCropperTemplateHelper {
    /// <inheritdoc cref="Umbraco.Extensions.FriendlyImageCropperTemplateExtensions.GetCropUrl(IPublishedContent, string, UrlMode)"/>
    string? GetCropUrl(
        IPublishedContent mediaItem,
        string cropAlias,
        UrlMode urlMode = UrlMode.Default);

    /// <inheritdoc cref="Umbraco.Extensions.FriendlyImageCropperTemplateExtensions.GetCropUrl(MediaWithCrops, string, UrlMode)"/>
    string? GetCropUrl(MediaWithCrops mediaWithCrops, string cropAlias, UrlMode urlMode = UrlMode.Default);

    /// <inheritdoc cref="Umbraco.Extensions.FriendlyImageCropperTemplateExtensions.GetCropUrl(IPublishedContent, ImageCropperValue, string, UrlMode)"/>
    string? GetCropUrl(
        IPublishedContent mediaItem,
        ImageCropperValue imageCropperValue,
        string cropAlias,
        UrlMode urlMode = UrlMode.Default);

    /// <inheritdoc cref="Umbraco.Extensions.FriendlyImageCropperTemplateExtensions.GetCropUrl(IPublishedContent, string, string, UrlMode)"/>
    string? GetCropUrl(
        IPublishedContent mediaItem,
        string propertyAlias,
        string cropAlias,
        UrlMode urlMode = UrlMode.Default);

    /// <inheritdoc cref="Umbraco.Extensions.FriendlyImageCropperTemplateExtensions.GetCropUrl(MediaWithCrops, string, string, UrlMode)"/>
    string? GetCropUrl(MediaWithCrops mediaWithCrops, string propertyAlias, string cropAlias, UrlMode urlMode = UrlMode.Default);

    /// <inheritdoc cref="Umbraco.Extensions.FriendlyImageCropperTemplateExtensions.GetCropUrl(IPublishedContent, int?, int?, string, string, int?, ImageCropMode?, ImageCropAnchor?, bool, bool, bool, string, UrlMode)"/>
    string? GetCropUrl(
        IPublishedContent mediaItem,
        int? width = null,
        int? height = null,
        string propertyAlias = Constants.Conventions.Media.File,
        string? cropAlias = null,
        int? quality = null,
        ImageCropMode? imageCropMode = null,
        ImageCropAnchor? imageCropAnchor = null,
        bool preferFocalPoint = false,
        bool useCropDimensions = false,
        bool cacheBuster = true,
        string? furtherOptions = null,
        UrlMode urlMode = UrlMode.Default);

    /// <inheritdoc cref="Umbraco.Extensions.FriendlyImageCropperTemplateExtensions.GetCropUrl(MediaWithCrops, int?, int?, string, string, int?, ImageCropMode?, ImageCropAnchor?, bool, bool, bool, string, UrlMode)"/>
    string? GetCropUrl(
        MediaWithCrops mediaWithCrops,
        int? width = null,
        int? height = null,
        string propertyAlias = Constants.Conventions.Media.File,
        string? cropAlias = null,
        int? quality = null,
        ImageCropMode? imageCropMode = null,
        ImageCropAnchor? imageCropAnchor = null,
        bool preferFocalPoint = false,
        bool useCropDimensions = false,
        bool cacheBuster = true,
        string? furtherOptions = null,
        UrlMode urlMode = UrlMode.Default);

    /// <inheritdoc cref="Umbraco.Extensions.FriendlyImageCropperTemplateExtensions.GetCropUrl(string, int?, int?, string, string, int?, ImageCropMode?, ImageCropAnchor?, bool, bool, string, string, int?, int?)"/>
    string? GetCropUrl(
        string imageUrl,
        int? width = null,
        int? height = null,
        string? imageCropperValue = null,
        string? cropAlias = null,
        int? quality = null,
        ImageCropMode? imageCropMode = null,
        ImageCropAnchor? imageCropAnchor = null,
        bool preferFocalPoint = false,
        bool useCropDimensions = false,
        string? cacheBusterValue = null,
        string? furtherOptions = null,
        int? sourceWidth = null,
        int? sourceHeight = null);

    /// <inheritdoc cref="Umbraco.Extensions.FriendlyImageCropperTemplateExtensions.GetCropUrl(string, ImageCropperValue, int?, int?, string, int?, ImageCropMode?, ImageCropAnchor?, bool, bool, string, string, int?, int?)"/>
    string? GetCropUrl(
        string imageUrl,
        ImageCropperValue cropDataSet,
        int? width = null,
        int? height = null,
        string? cropAlias = null,
        int? quality = null,
        ImageCropMode? imageCropMode = null,
        ImageCropAnchor? imageCropAnchor = null,
        bool preferFocalPoint = false,
        bool useCropDimensions = false,
        string? cacheBusterValue = null,
        string? furtherOptions = null,
        int? sourceWidth = null,
        int? sourceHeight = null);
}
