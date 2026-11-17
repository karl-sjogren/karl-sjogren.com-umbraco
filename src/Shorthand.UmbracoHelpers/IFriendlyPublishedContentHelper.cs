using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace Shorthand.UmbracoHelpers;

public interface IFriendlyPublishedContentHelper {
    /// <inheritdoc cref="Umbraco.Extensions.FriendlyPublishedContentExtensions.CreateModel(IPublishedContent)"/>
    IPublishedContent? CreateModel(
        IPublishedContent content);

    /// <inheritdoc cref="Umbraco.Extensions.FriendlyPublishedContentExtensions.Name(IPublishedContent, string)"/>
    string Name(
        IPublishedContent content,
        string? culture = null);

    /// <inheritdoc cref="Umbraco.Extensions.FriendlyPublishedContentExtensions.UrlSegment(IPublishedContent, string)"/>
    string? UrlSegment(
        IPublishedContent content,
        string? culture = null);

    /// <inheritdoc cref="Umbraco.Extensions.FriendlyPublishedContentExtensions.CultureDate(IPublishedContent, string)"/>
    DateTime CultureDate(
        IPublishedContent content,
        string? culture = null);

    /// <inheritdoc cref="Umbraco.Extensions.FriendlyPublishedContentExtensions.GetTemplateAlias(IPublishedContent)"/>
    string GetTemplateAlias(IPublishedContent content);

    /// <inheritdoc cref="Umbraco.Extensions.FriendlyPublishedContentExtensions.IsAllowedTemplate(IPublishedContent, int)"/>
    bool IsAllowedTemplate(IPublishedContent content, int templateId);

    /// <inheritdoc cref="Umbraco.Extensions.FriendlyPublishedContentExtensions.IsAllowedTemplate(IPublishedContent, string)"/>
    bool IsAllowedTemplate(IPublishedContent content, string templateAlias);

    /// <inheritdoc cref="Umbraco.Extensions.FriendlyPublishedContentExtensions.IsAllowedTemplate(IPublishedContent, bool, bool, int)"/>
    bool IsAllowedTemplate(
        IPublishedContent content,
        bool disableAlternativeTemplates,
        bool validateAlternativeTemplates,
        int templateId);

    /// <inheritdoc cref="Umbraco.Extensions.FriendlyPublishedContentExtensions.IsAllowedTemplate(IPublishedContent, bool, bool, string)"/>
    bool IsAllowedTemplate(
        IPublishedContent content,
        bool disableAlternativeTemplates,
        bool validateAlternativeTemplates,
        string templateAlias);

    /// <inheritdoc cref="Umbraco.Extensions.FriendlyPublishedContentExtensions.HasValue(IPublishedContent, string, string, string, Fallback)"/>
    bool HasValue(
        IPublishedContent content,
        string alias,
        string? culture = null,
        string? segment = null,
        Fallback fallback = default);

    /// <inheritdoc cref="Umbraco.Extensions.FriendlyPublishedContentExtensions.Value(IPublishedContent, string, string, string, Fallback, object)"/>
    object? Value(IPublishedContent content, string alias, string? culture = null, string? segment = null, Fallback fallback = default, object? defaultValue = default);

    /// <inheritdoc cref="Umbraco.Extensions.FriendlyPublishedContentExtensions.Value{T}(IPublishedContent, string, string, string, Fallback, T)"/>
    T? Value<T>(IPublishedContent content, string alias, string? culture = null, string? segment = null, Fallback fallback = default, T? defaultValue = default);

    /// <inheritdoc cref="Umbraco.Extensions.FriendlyPublishedContentExtensions.Root(IPublishedContent)"/>
    IPublishedContent Root(IPublishedContent content);

    /// <inheritdoc cref="Umbraco.Extensions.FriendlyPublishedContentExtensions.Root{T}(IPublishedContent)"/>
    T? Root<T>(IPublishedContent content)
        where T : class, IPublishedContent;

    /// <inheritdoc cref="Umbraco.Extensions.FriendlyPublishedContentExtensions.Parent{T}(IPublishedContent)"/>
    T? Parent<T>(IPublishedContent content)
        where T : class, IPublishedContent;

    /// <inheritdoc cref="Umbraco.Extensions.FriendlyPublishedContentExtensions.Parent(IPublishedContent)"/>
    IPublishedContent? Parent(IPublishedContent content);

    /// <inheritdoc cref="Umbraco.Extensions.FriendlyPublishedContentExtensions.Ancestors(IPublishedContent)"/>
    IEnumerable<IPublishedContent> Ancestors(IPublishedContent content);

    /// <inheritdoc cref="Umbraco.Extensions.FriendlyPublishedContentExtensions.AncestorsOrSelf(IPublishedContent)"/>
    IEnumerable<IPublishedContent> AncestorsOrSelf(IPublishedContent content);

    /// <inheritdoc cref="Umbraco.Extensions.FriendlyPublishedContentExtensions.AncestorsOrSelf{T}(IPublishedContent)"/>
    IEnumerable<T> AncestorsOrSelf<T>(IPublishedContent content)
        where T : class, IPublishedContent;

    /// <inheritdoc cref="Umbraco.Extensions.FriendlyPublishedContentExtensions.Ancestor(IPublishedContent)"/>
    IPublishedContent? Ancestor(IPublishedContent content);

    /// <inheritdoc cref="Umbraco.Extensions.FriendlyPublishedContentExtensions.Ancestor{T}(IPublishedContent)"/>
    T? Ancestor<T>(IPublishedContent content)
        where T : class, IPublishedContent;

    /// <inheritdoc cref="Umbraco.Extensions.FriendlyPublishedContentExtensions.AncestorOrSelf{T}(IPublishedContent)"/>
    T? AncestorOrSelf<T>(IPublishedContent content)
        where T : class, IPublishedContent;

    /// <inheritdoc cref="Umbraco.Extensions.FriendlyPublishedContentExtensions.DescendantsOrSelfOfType(IEnumerable{IPublishedContent}, string, string)"/>
    IEnumerable<IPublishedContent> DescendantsOrSelfOfType(
        IEnumerable<IPublishedContent> parentNodes, string docTypeAlias, string? culture = null);

    /// <inheritdoc cref="Umbraco.Extensions.FriendlyPublishedContentExtensions.DescendantsOrSelf{T}(IEnumerable{IPublishedContent}, string)"/>
    IEnumerable<T> DescendantsOrSelf<T>(
        IEnumerable<IPublishedContent> parentNodes,
        string? culture = null)
        where T : class, IPublishedContent;

    /// <inheritdoc cref="Umbraco.Extensions.FriendlyPublishedContentExtensions.Descendants(IPublishedContent, string)"/>
    IEnumerable<IPublishedContent> Descendants(IPublishedContent content, string? culture = null);

    /// <inheritdoc cref="Umbraco.Extensions.FriendlyPublishedContentExtensions.Descendants(IPublishedContent, int, string)"/>
    IEnumerable<IPublishedContent> Descendants(IPublishedContent content, int level, string? culture = null);

    /// <inheritdoc cref="Umbraco.Extensions.FriendlyPublishedContentExtensions.DescendantsOfType(IPublishedContent, string, string)"/>
    IEnumerable<IPublishedContent> DescendantsOfType(IPublishedContent content, string contentTypeAlias, string? culture = null);

    /// <inheritdoc cref="Umbraco.Extensions.FriendlyPublishedContentExtensions.Descendants{T}(IPublishedContent, string)"/>
    IEnumerable<T> Descendants<T>(IPublishedContent content, string? culture = null)
        where T : class, IPublishedContent;

    /// <inheritdoc cref="Umbraco.Extensions.FriendlyPublishedContentExtensions.Descendants{T}(IPublishedContent, int, string)"/>
    IEnumerable<T> Descendants<T>(IPublishedContent content, int level, string? culture = null)
        where T : class, IPublishedContent;

    /// <inheritdoc cref="Umbraco.Extensions.FriendlyPublishedContentExtensions.DescendantsOrSelf(IPublishedContent, string)"/>
    IEnumerable<IPublishedContent> DescendantsOrSelf(
        IPublishedContent content,
        string? culture = null);

    /// <inheritdoc cref="Umbraco.Extensions.FriendlyPublishedContentExtensions.DescendantsOrSelf(IPublishedContent, int, string)"/>
    IEnumerable<IPublishedContent> DescendantsOrSelf(IPublishedContent content, int level, string? culture = null);

    /// <inheritdoc cref="Umbraco.Extensions.FriendlyPublishedContentExtensions.DescendantsOrSelfOfType(IPublishedContent, string, string)"/>
    IEnumerable<IPublishedContent> DescendantsOrSelfOfType(IPublishedContent content, string contentTypeAlias, string? culture = null);

    /// <inheritdoc cref="Umbraco.Extensions.FriendlyPublishedContentExtensions.DescendantsOrSelf{T}(IPublishedContent, string)"/>
    IEnumerable<T> DescendantsOrSelf<T>(IPublishedContent content, string? culture = null)
        where T : class, IPublishedContent;

    /// <inheritdoc cref="Umbraco.Extensions.FriendlyPublishedContentExtensions.DescendantsOrSelf{T}(IPublishedContent, int, string)"/>
    IEnumerable<T> DescendantsOrSelf<T>(IPublishedContent content, int level, string? culture = null)
        where T : class, IPublishedContent;

    /// <inheritdoc cref="Umbraco.Extensions.FriendlyPublishedContentExtensions.Descendant(IPublishedContent, string)"/>
    IPublishedContent? Descendant(IPublishedContent content, string? culture = null);

    /// <inheritdoc cref="Umbraco.Extensions.FriendlyPublishedContentExtensions.Descendant(IPublishedContent, int, string)"/>
    IPublishedContent? Descendant(IPublishedContent content, int level, string? culture = null);

    /// <inheritdoc cref="Umbraco.Extensions.FriendlyPublishedContentExtensions.DescendantOfType(IPublishedContent, string, string)"/>
    IPublishedContent? DescendantOfType(IPublishedContent content, string contentTypeAlias, string? culture = null);

    /// <inheritdoc cref="Umbraco.Extensions.FriendlyPublishedContentExtensions.Descendant{T}(IPublishedContent, string)"/>
    T? Descendant<T>(IPublishedContent content, string? culture = null)
        where T : class, IPublishedContent;

    /// <inheritdoc cref="Umbraco.Extensions.FriendlyPublishedContentExtensions.Descendant{T}(IPublishedContent, int, string)"/>
    T? Descendant<T>(IPublishedContent content, int level, string? culture = null)
        where T : class, IPublishedContent;

    /// <inheritdoc cref="Umbraco.Extensions.FriendlyPublishedContentExtensions.DescendantOrSelf(IPublishedContent, string)"/>
    IPublishedContent DescendantOrSelf(IPublishedContent content, string? culture = null);

    /// <inheritdoc cref="Umbraco.Extensions.FriendlyPublishedContentExtensions.DescendantOrSelf(IPublishedContent, int, string)"/>
    IPublishedContent? DescendantOrSelf(IPublishedContent content, int level, string? culture = null);

    /// <inheritdoc cref="Umbraco.Extensions.FriendlyPublishedContentExtensions.DescendantOrSelfOfType(IPublishedContent, string, string)"/>
    IPublishedContent? DescendantOrSelfOfType(IPublishedContent content, string contentTypeAlias, string? culture = null);

    /// <inheritdoc cref="Umbraco.Extensions.FriendlyPublishedContentExtensions.DescendantOrSelf{T}(IPublishedContent, string)"/>
    T? DescendantOrSelf<T>(IPublishedContent content, string? culture = null)
        where T : class, IPublishedContent;

    /// <inheritdoc cref="Umbraco.Extensions.FriendlyPublishedContentExtensions.DescendantOrSelf{T}(IPublishedContent, int, string)"/>
    T? DescendantOrSelf<T>(IPublishedContent content, int level, string? culture = null)
        where T : class, IPublishedContent;

    /// <inheritdoc cref="Umbraco.Extensions.FriendlyPublishedContentExtensions.Children(IPublishedContent, string)"/>
    IEnumerable<IPublishedContent> Children(IPublishedContent content, string? culture = null);

    /// <inheritdoc cref="Umbraco.Extensions.FriendlyPublishedContentExtensions.Children(IPublishedContent, Func{IPublishedContent,bool}, string)"/>
    IEnumerable<IPublishedContent> Children(
        IPublishedContent content,
        Func<IPublishedContent, bool> predicate,
        string? culture = null);

    /// <inheritdoc cref="Umbraco.Extensions.FriendlyPublishedContentExtensions.ChildrenOfType(IPublishedContent, string, string)"/>
    IEnumerable<IPublishedContent> ChildrenOfType(IPublishedContent content, string contentTypeAlias, string? culture = null);

    /// <inheritdoc cref="Umbraco.Extensions.FriendlyPublishedContentExtensions.Children{T}(IPublishedContent, string)"/>
    IEnumerable<T> Children<T>(IPublishedContent content, string? culture = null)
        where T : class, IPublishedContent;

    /// <inheritdoc cref="Umbraco.Extensions.FriendlyPublishedContentExtensions.FirstChild(IPublishedContent, string)"/>
    IPublishedContent? FirstChild(IPublishedContent content, string? culture = null);

    /// <inheritdoc cref="Umbraco.Extensions.FriendlyPublishedContentExtensions.FirstChildOfType(IPublishedContent, string, string)"/>
    IPublishedContent? FirstChildOfType(IPublishedContent content, string contentTypeAlias, string? culture = null);

    /// <inheritdoc cref="Umbraco.Extensions.FriendlyPublishedContentExtensions.FirstChild(IPublishedContent, Func{IPublishedContent,bool}, string)"/>
    IPublishedContent? FirstChild(IPublishedContent content, Func<IPublishedContent, bool> predicate, string? culture = null);

    /// <inheritdoc cref="Umbraco.Extensions.FriendlyPublishedContentExtensions.FirstChild(IPublishedContent, Guid, string)"/>
    IPublishedContent? FirstChild(IPublishedContent content, Guid uniqueId, string? culture = null);

    /// <inheritdoc cref="Umbraco.Extensions.FriendlyPublishedContentExtensions.FirstChild{T}(IPublishedContent, string)"/>
    T? FirstChild<T>(IPublishedContent content, string? culture = null)
        where T : class, IPublishedContent;

    /// <inheritdoc cref="Umbraco.Extensions.FriendlyPublishedContentExtensions.FirstChild{T}(IPublishedContent, Func{T,bool}, string)"/>
    T? FirstChild<T>(IPublishedContent content, Func<T, bool> predicate, string? culture = null)
        where T : class, IPublishedContent;

    /// <inheritdoc cref="Umbraco.Extensions.FriendlyPublishedContentExtensions.Siblings(IPublishedContent, string)"/>
    IEnumerable<IPublishedContent> Siblings(IPublishedContent content, string? culture = null);

    /// <inheritdoc cref="Umbraco.Extensions.FriendlyPublishedContentExtensions.SiblingsOfType(IPublishedContent, string, string)"/>
    IEnumerable<IPublishedContent> SiblingsOfType(IPublishedContent content, string contentTypeAlias, string? culture = null);

    /// <inheritdoc cref="Umbraco.Extensions.FriendlyPublishedContentExtensions.Siblings{T}(IPublishedContent, string)"/>
    IEnumerable<T> Siblings<T>(IPublishedContent content, string? culture = null)
        where T : class, IPublishedContent;

    /// <inheritdoc cref="Umbraco.Extensions.FriendlyPublishedContentExtensions.SiblingsAndSelf(IPublishedContent, string)"/>
    IEnumerable<IPublishedContent>? SiblingsAndSelf(
        IPublishedContent content,
        string? culture = null);

    /// <inheritdoc cref="Umbraco.Extensions.FriendlyPublishedContentExtensions.SiblingsAndSelfOfType(IPublishedContent, string, string)"/>
    IEnumerable<IPublishedContent> SiblingsAndSelfOfType(
        IPublishedContent content,
        string contentTypeAlias,
        string? culture = null);

    /// <inheritdoc cref="Umbraco.Extensions.FriendlyPublishedContentExtensions.SiblingsAndSelf{T}(IPublishedContent, string)"/>
    IEnumerable<T> SiblingsAndSelf<T>(IPublishedContent content, string? culture = null)
        where T : class, IPublishedContent;

    /// <inheritdoc cref="Umbraco.Extensions.FriendlyPublishedContentExtensions.Url(IPublishedContent, string, UrlMode)"/>
    string Url(IPublishedContent content, string? culture = null, UrlMode mode = UrlMode.Default);

    /// <inheritdoc cref="Umbraco.Extensions.FriendlyPublishedContentExtensions.MediaUrl(IPublishedContent, string, UrlMode, string)"/>
    string MediaUrl(
        IPublishedContent content,
        string? culture = null,
        UrlMode mode = UrlMode.Default,
        string propertyAlias = Constants.Conventions.Media.File);

    /// <inheritdoc cref="Umbraco.Extensions.FriendlyPublishedContentExtensions.CreatorName(IPublishedContent)"/>
    string? CreatorName(IPublishedContent content);

    /// <inheritdoc cref="Umbraco.Extensions.FriendlyPublishedContentExtensions.WriterName(IPublishedContent)"/>
    string? WriterName(IPublishedContent content);

    /// <inheritdoc cref="Umbraco.Extensions.FriendlyPublishedContentExtensions.GetCultureFromDomains(IPublishedContent, Uri)"/>
    string? GetCultureFromDomains(
        IPublishedContent content,
        Uri? current = null);
}
