using System.Linq.Expressions;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Services;

namespace Shorthand.UmbracoHelpers;

public class FriendlyPublishedElementHelper : IFriendlyPublishedElementHelper {
    private readonly IPublishedValueFallback _publishedValueFallback;
    private readonly IVariationContextAccessor _variationContextAccessor;
    private readonly IUserService _userService;

    public FriendlyPublishedElementHelper(
            IPublishedValueFallback publishedValueFallback,
            IVariationContextAccessor variationContextAccessor,
            IUserService userService) {
        _publishedValueFallback = publishedValueFallback;
        _variationContextAccessor = variationContextAccessor;
        _userService = userService;
    }

    public string Name(
        IPublishedElement content,
        string? culture = null)
        => content.Name(_variationContextAccessor, culture);

    public DateTime CultureDate(
        IPublishedElement content,
        string? culture = null)
        => content.CultureDate(_variationContextAccessor, culture);

    public object? Value(
        IPublishedElement content,
        string alias,
        string? culture = null,
        string? segment = null,
        Fallback fallback = default,
        object? defaultValue = default)
        => content.Value(_publishedValueFallback, alias, culture, segment, fallback, defaultValue);

    public T? Value<T>(
        IPublishedElement content,
        string alias,
        string? culture = null,
        string? segment = null,
        Fallback fallback = default,
        T? defaultValue = default)
        => content.Value(_publishedValueFallback, alias, culture, segment, fallback, defaultValue);

    public bool IsVisible(IPublishedElement content)
        => content.IsVisible(_publishedValueFallback);

    public string? CreatorName(IPublishedElement content)
        =>
        content.CreatorName(_userService);

    public string? WriterName(IPublishedElement content)
        =>
        content.WriterName(_userService);

    public TValue? ValueFor<TModel, TValue>(
        TModel model,
        Expression<Func<TModel, TValue>> property,
        string? culture = null,
        string? segment = null,
        Fallback fallback = default,
        TValue? defaultValue = default)
        where TModel : IPublishedElement
        =>
        model.ValueFor(_publishedValueFallback, property, culture, segment, fallback);
}
