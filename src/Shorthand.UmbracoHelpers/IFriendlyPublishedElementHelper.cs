using System.Linq.Expressions;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace Shorthand.UmbracoHelpers;

public interface IFriendlyPublishedElementHelper {
    /// <inheritdoc cref="Umbraco.Extensions.FriendlyPublishedElementExtensions.Name(IPublishedElement, string)"/>
    string Name(
        IPublishedElement content,
        string? culture = null);

    /// <inheritdoc cref="Umbraco.Extensions.FriendlyPublishedElementExtensions.CultureDate(IPublishedElement, string)"/>
    DateTime CultureDate(
        IPublishedElement content,
        string? culture = null);

    /// <inheritdoc cref="Umbraco.Extensions.FriendlyPublishedElementExtensions.Value(IPublishedElement, string, string, string, Fallback, object)"/>
    object? Value(
        IPublishedElement content,
        string alias,
        string? culture = null,
        string? segment = null,
        Fallback fallback = default,
        object? defaultValue = default);

    /// <inheritdoc cref="Umbraco.Extensions.FriendlyPublishedElementExtensions.Value{T}(IPublishedElement, string, string, string, Fallback, T)"/>
    T? Value<T>(
        IPublishedElement content,
        string alias,
        string? culture = null,
        string? segment = null,
        Fallback fallback = default,
        T? defaultValue = default);

    /// <inheritdoc cref="Umbraco.Extensions.FriendlyPublishedElementExtensions.IsVisible(IPublishedElement)"/>
    bool IsVisible(IPublishedElement content);

    /// <inheritdoc cref="Umbraco.Extensions.FriendlyPublishedElementExtensions.CreatorName(IPublishedElement)"/>
    string? CreatorName(IPublishedElement content);

    /// <inheritdoc cref="Umbraco.Extensions.FriendlyPublishedElementExtensions.WriterName(IPublishedElement)"/>
    string? WriterName(IPublishedElement content);

    /// <inheritdoc cref="Umbraco.Extensions.FriendlyPublishedElementExtensions.ValueFor{TModel,TValue}(TModel, Expression{Func{TModel,TValue}}, string, string, Fallback, TValue)"/>
    TValue? ValueFor<TModel, TValue>(
        TModel model,
        Expression<Func<TModel, TValue>> property,
        string? culture = null,
        string? segment = null,
        Fallback fallback = default,
        TValue? defaultValue = default)
        where TModel : IPublishedElement;
}
