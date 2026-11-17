using System.Globalization;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Media;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PropertyEditors.ValueConverters;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Serialization;

namespace Shorthand.UmbracoHelpers;

public class FriendlyImageCropperTemplateHelper : IFriendlyImageCropperTemplateHelper {
    private readonly IImageUrlGenerator _imageUrlGenerator;
    private readonly IPublishedValueFallback _publishedValueFallback;
    private readonly IPublishedUrlProvider _publishedUrlProvider;
    private readonly IJsonSerializer _jsonSerializer;
    private readonly ILogger<FriendlyImageCropperTemplateHelper> _logger;

    public FriendlyImageCropperTemplateHelper(
            IImageUrlGenerator imageUrlGenerator,
            IPublishedValueFallback publishedValueFallback,
            IPublishedUrlProvider publishedUrlProvider,
            IJsonSerializer jsonSerializer,
            ILogger<FriendlyImageCropperTemplateHelper> logger) {
        _imageUrlGenerator = imageUrlGenerator;
        _publishedValueFallback = publishedValueFallback;
        _publishedUrlProvider = publishedUrlProvider;
        _jsonSerializer = jsonSerializer;
        _logger = logger;
    }

    public string? GetCropUrl(
        IPublishedContent mediaItem,
        string cropAlias,
        UrlMode urlMode = UrlMode.Default)
        => GetCropUrlCore(mediaItem, null, false, cropAlias: cropAlias, useCropDimensions: true, urlMode: urlMode);

    public string? GetCropUrl(MediaWithCrops mediaWithCrops, string cropAlias, UrlMode urlMode = UrlMode.Default)
        => GetCropUrl(mediaWithCrops, cropAlias: cropAlias, useCropDimensions: true, urlMode: urlMode);

    public string? GetCropUrl(
        IPublishedContent mediaItem,
        ImageCropperValue imageCropperValue,
        string cropAlias,
        UrlMode urlMode = UrlMode.Default)
        => GetCropUrlCore(mediaItem, imageCropperValue, true, cropAlias: cropAlias, useCropDimensions: true, urlMode: urlMode);

    public string? GetCropUrl(
        IPublishedContent mediaItem,
        string propertyAlias,
        string cropAlias,
        UrlMode urlMode = UrlMode.Default)
        => GetCropUrlCore(mediaItem, null, false, propertyAlias: propertyAlias, cropAlias: cropAlias, useCropDimensions: true, urlMode: urlMode);

    public string? GetCropUrl(MediaWithCrops mediaWithCrops, string propertyAlias, string cropAlias, UrlMode urlMode = UrlMode.Default)
        => GetCropUrl(mediaWithCrops, propertyAlias: propertyAlias, cropAlias: cropAlias, useCropDimensions: true, urlMode: urlMode);

    public string? GetCropUrl(
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
        UrlMode urlMode = UrlMode.Default)
        => GetCropUrlCore(
            mediaItem,
            null,
            false,
            width,
            height,
            propertyAlias,
            cropAlias,
            quality,
            imageCropMode,
            imageCropAnchor,
            preferFocalPoint,
            useCropDimensions,
            cacheBuster,
            furtherOptions,
            urlMode);

    public string? GetCropUrl(
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
        UrlMode urlMode = UrlMode.Default) {
        ArgumentNullException.ThrowIfNull(mediaWithCrops);
        return GetCropUrlCore(
            mediaWithCrops.Content,
            mediaWithCrops.LocalCrops,
            false,
            width,
            height,
            propertyAlias,
            cropAlias,
            quality,
            imageCropMode,
            imageCropAnchor,
            preferFocalPoint,
            useCropDimensions,
            cacheBuster,
            furtherOptions,
            urlMode);
    }

    public string? GetCropUrl(
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
        int? sourceHeight = null) {
        if(string.IsNullOrWhiteSpace(imageUrl)) {
            return null;
        }

        ImageCropperValue? crops = null;
        if(!string.IsNullOrEmpty(imageCropperValue) && imageCropperValue.DetectIsJson()
            && (imageCropMode == ImageCropMode.Crop || imageCropMode == null)) {
            crops = DeserializeImageCropperValue(imageCropperValue);
        }

        return imageUrl.GetCropUrl(
            _imageUrlGenerator,
            crops,
            width,
            height,
            cropAlias,
            quality,
            imageCropMode,
            imageCropAnchor,
            preferFocalPoint,
            useCropDimensions,
            cacheBusterValue,
            furtherOptions,
            sourceWidth,
            sourceHeight);
    }

    public string? GetCropUrl(
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
        int? sourceHeight = null)
        => imageUrl.GetCropUrl(
            _imageUrlGenerator,
            cropDataSet,
            width,
            height,
            cropAlias,
            quality,
            imageCropMode,
            imageCropAnchor,
            preferFocalPoint,
            useCropDimensions,
            cacheBusterValue,
            furtherOptions,
            sourceWidth,
            sourceHeight);

    private ImageCropperValue DeserializeImageCropperValue(string json) {
        try {
            return _jsonSerializer.Deserialize<ImageCropperValue>(json) ?? new ImageCropperValue();
        } catch(Exception ex) {
            _logger.LogError(ex, "Could not parse the json string: {Json}", json);
            return new ImageCropperValue();
        }
    }

    private string? GetCropUrlCore(
        IPublishedContent mediaItem,
        ImageCropperValue? localCrops,
        bool localCropsOnly,
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
        UrlMode urlMode = UrlMode.Default) {
        ArgumentNullException.ThrowIfNull(mediaItem);

        if(!mediaItem.HasProperty(propertyAlias) || !mediaItem.HasValue(_publishedValueFallback, propertyAlias)) {
            return null;
        }

        var mediaItemUrl = mediaItem.MediaUrl(_publishedUrlProvider, propertyAlias: propertyAlias, mode: urlMode);

        if(!localCropsOnly && (imageCropMode == ImageCropMode.Crop || imageCropMode == null)) {
            var cropperValue = mediaItem.Value(_publishedValueFallback, propertyAlias);
            var mediaCrops = cropperValue as ImageCropperValue;
            if(mediaCrops == null && cropperValue is string json && !string.IsNullOrEmpty(json) && json.DetectIsJson()) {
                mediaCrops = DeserializeImageCropperValue(json);
            }

            if(localCrops == null) {
                localCrops = mediaCrops;
            } else if(mediaCrops != null) {
                localCrops = localCrops.Merge(mediaCrops);
            }
        }

        var cacheBusterValue = cacheBuster ? mediaItem.UpdateDate.ToFileTimeUtc().ToString("x", CultureInfo.InvariantCulture) : null;
        var sourceWidth = mediaItem.Value<int?>(_publishedValueFallback, Constants.Conventions.Media.Width);
        var sourceHeight = mediaItem.Value<int?>(_publishedValueFallback, Constants.Conventions.Media.Height);

        return mediaItemUrl.GetCropUrl(
            _imageUrlGenerator,
            localCrops,
            width,
            height,
            cropAlias,
            quality,
            imageCropMode,
            imageCropAnchor,
            preferFocalPoint,
            useCropDimensions,
            cacheBusterValue,
            furtherOptions,
            sourceWidth,
            sourceHeight);
    }
}
