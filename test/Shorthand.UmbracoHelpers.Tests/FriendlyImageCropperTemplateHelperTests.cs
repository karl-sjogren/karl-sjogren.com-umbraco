using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Media;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PropertyEditors.ValueConverters;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Serialization;

namespace Shorthand.UmbracoHelpers.Tests;

public class FriendlyImageCropperTemplateHelperTests {
    [Fact]
    public void CropJsonUsesInjectedSerializerAndGenerator() {
        // Arrange
        var generator = A.Fake<IImageUrlGenerator>();
        var serializer = A.Fake<IJsonSerializer>();
        var crops = new ImageCropperValue();

        A.CallTo(() => serializer.Deserialize<ImageCropperValue>("{}")).Returns(crops);
        A.CallTo(() => generator.GetImageUrl(A<ImageUrlGenerationOptions>._)).Returns("generated");

        var helper = new FriendlyImageCropperTemplateHelper(
            generator,
            A.Fake<IPublishedValueFallback>(),
            A.Fake<IPublishedUrlProvider>(),
            serializer,
            NullLogger<FriendlyImageCropperTemplateHelper>.Instance);

        // Act & assert
        helper.GetCropUrl("/image.jpg", width: 120, height: 80, imageCropperValue: "{}").ShouldBe("generated");

        // Assert
        A.CallTo(() => serializer.Deserialize<ImageCropperValue>("{}")).MustHaveHappenedOnceExactly();
        A.CallTo(() => generator.GetImageUrl(
            A<ImageUrlGenerationOptions>.That.Matches(options => options.Width == 120 && options.Height == 80)))
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public void MediaCropUsesInjectedFallbackAndSerializerForMediaAndDimensions() {
        // Arrange
        var generator = A.Fake<IImageUrlGenerator>();
        var serializer = A.Fake<IJsonSerializer>();
        var fallback = A.Fake<IPublishedValueFallback>();
        var urlProvider = A.Fake<IPublishedUrlProvider>();
        var media = A.Fake<IPublishedContent>();
        var mediaType = A.Fake<IPublishedContentType>();
        var file = A.Fake<IPublishedProperty>();
        var propertyType = A.Fake<IPublishedPropertyType>();

        A.CallTo(() => media.ContentType).Returns(mediaType);
        A.CallTo(() => mediaType.GetPropertyType(Constants.Conventions.Media.File)).Returns(propertyType);
        A.CallTo(() => media.GetProperty(Constants.Conventions.Media.File)).Returns(file);
        A.CallTo(() => file.HasValue(null, null)).Returns(true);
        A.CallTo(() => file.GetValue(null, null)).Returns("{}");
        A.CallTo(() => urlProvider.GetMediaUrl(media, UrlMode.Absolute, null, Constants.Conventions.Media.File, null)).Returns("/image.jpg");
        A.CallTo(() => serializer.Deserialize<ImageCropperValue>("{}")).Returns(new ImageCropperValue());
        A.CallTo(() => generator.GetImageUrl(A<ImageUrlGenerationOptions>._)).Returns("generated");

        var helper = new FriendlyImageCropperTemplateHelper(
            generator,
            fallback,
            urlProvider,
            serializer,
            NullLogger<FriendlyImageCropperTemplateHelper>.Instance);

        // Act & assert
        helper.GetCropUrl(media, width: 120, cacheBuster: false, urlMode: UrlMode.Absolute).ShouldBe("generated");

        // Assert
        A.CallTo(() => serializer.Deserialize<ImageCropperValue>("{}")).MustHaveHappenedOnceExactly();
        A.CallTo(() => generator.GetImageUrl(
            A<ImageUrlGenerationOptions>.That.Matches(options => options.Width == 120 && options.CacheBusterValue == null)))
            .MustHaveHappenedOnceExactly();
        A.CallTo(() => media.GetProperty(Constants.Conventions.Media.Width)).MustHaveHappenedOnceExactly();
        A.CallTo(() => media.GetProperty(Constants.Conventions.Media.Height)).MustHaveHappenedOnceExactly();
    }
}
