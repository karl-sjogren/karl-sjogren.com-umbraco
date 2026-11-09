using Shorthand.UmbracoCodeFirst.Attributes;

namespace UmbracoBlog.Core.Documents;

[AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
internal sealed class ContentTabAttribute()
    : UCFTabAttribute("Content", 0);
