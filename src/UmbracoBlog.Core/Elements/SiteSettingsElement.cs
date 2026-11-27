using Shorthand.UmbracoCodeFirst;
using Shorthand.UmbracoCodeFirst.Attributes;
using UmbracoBlog.Core.DataTypes;

namespace UmbracoBlog.Core.Elements;

[UCFElementType("Site settings", AllowInLibrary = true, Icon = UmbracoIcons.Settings)]
[UCFFolderPath("Elements")]
public partial class SiteSettingsElement {
    [UCFProperty("Site name",
        UmbracoBlogTextDataType.Alias,
        Mandatory = true)]
    public partial string SiteName { get; }

    [UCFProperty("Site owner",
        UmbracoBlogTextDataType.Alias,
        Mandatory = true)]
    public partial string SiteOwner { get; }

    [UCFProperty("Site tagline",
        UmbracoBlogTextDataType.Alias)]
    public partial string SiteTagline { get; }

    [UCFProperty("Github link",
        UmbracoBlogTextDataType.Alias)]
    public partial string GithubLink { get; }

    [UCFProperty("Stackoverflow link",
        UmbracoBlogTextDataType.Alias)]
    public partial string StackoverflowLink { get; }

    [UCFProperty("Email",
        UmbracoBlogEmailDataType.Alias)]
    public partial string Email { get; }
}
